using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;
using StudyBuddyApp.Services.Notifications;

namespace StudyBuddyApp.Services.Observers
{
    public class SesijaObavjestenjeObserver : ISesijaObserver
    {
        private readonly ApplicationDbContext _context;
        private readonly ObavjestenjeFactory _obavjestenjeFactory;
        private readonly IObavjestenjeSender _obavjestenjeSender;

        public SesijaObavjestenjeObserver(
            ApplicationDbContext context,
            ObavjestenjeFactory obavjestenjeFactory,
            IObavjestenjeSender obavjestenjeSender)
        {
            _context = context;
            _obavjestenjeFactory = obavjestenjeFactory;
            _obavjestenjeSender = obavjestenjeSender;
        }

        public async Task AzurirajAsync(SesijaPromjenaInfo promjena)
        {
            var sesija = await _context.SesijeUcenja
                .Include(s => s.Lokacija)
                .Include(s => s.Predmet)
                .FirstOrDefaultAsync(s => s.IdSesije == promjena.SesijaId);

            if (sesija == null)
            {
                return;
            }

            var prijavljeniKorisnici = await _context.PrijaveNaSesije
                .Include(p => p.Korisnik)
                .Where(p =>
                    p.SesijaId == promjena.SesijaId &&
                    p.StatusPrijave == StatusPrijave.Prijavljen)
                .Select(p => p.Korisnik)
                .Where(k => k != null)
                .ToListAsync();

            if (!prijavljeniKorisnici.Any())
            {
                return;
            }

            foreach (var korisnik in prijavljeniKorisnici)
            {
                if (korisnik == null)
                {
                    continue;
                }

                var obavjestenje = _obavjestenjeFactory.KreirajObavjestenje(
                    TipObavjestenja.PromjenaTermina,
                    korisnik,
                    sesija
                );

                obavjestenje.Korisnik = korisnik;
                obavjestenje.Sadrzaj = KreirajSadrzajPromjene(promjena, sesija);

                _context.Obavjestenja.Add(obavjestenje);

                await _obavjestenjeSender.PosaljiAsync(obavjestenje);
            }

            await _context.SaveChangesAsync();
        }

        private string KreirajSadrzajPromjene(SesijaPromjenaInfo promjena, SesijaUcenja sesija)
        {
            var dijelovi = new List<string>
            {
                $"Sesija \"{sesija.Naziv}\" je izmijenjena."
            };

            if (promjena.DatumPromijenjen)
            {
                dijelovi.Add($"Novi termin je {promjena.NoviDatumVrijeme.ToLocalTime():dd.MM.yyyy. HH:mm}.");
            }

            if (promjena.LokacijaPromijenjena)
            {
                dijelovi.Add($"Nova lokacija je {sesija.Lokacija?.Naziv ?? "nije navedena"}.");
            }

            if (promjena.StatusPromijenjen)
            {
                dijelovi.Add($"Novi status sesije je {promjena.NoviStatus}.");
            }

            return string.Join(" ", dijelovi);
        }
    }
}
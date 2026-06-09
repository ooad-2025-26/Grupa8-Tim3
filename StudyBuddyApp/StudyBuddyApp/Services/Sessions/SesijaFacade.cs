using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;
using StudyBuddyApp.Services.Notifications;

namespace StudyBuddyApp.Services.Sessions
{
    public class SesijaFacade
    {
        private readonly ApplicationDbContext _context;
        private readonly ObavjestenjeFactory _obavjestenjeFactory;
        private readonly IObavjestenjeSender _obavjestenjeSender;

        public SesijaFacade(
            ApplicationDbContext context,
            ObavjestenjeFactory obavjestenjeFactory,
            IObavjestenjeSender obavjestenjeSender)
        {
            _context = context;
            _obavjestenjeFactory = obavjestenjeFactory;
            _obavjestenjeSender = obavjestenjeSender;
        }

        public async Task<SessionOperationResult> KreirajSesijuAsync(
            SesijaUcenja podaci,
            Korisnik kreator)
        {
            if (kreator == null)
            {
                return SessionOperationResult.Greska("Korisnik nije pronađen.");
            }

            if (podaci.MaksimalanBrojUcesnika <= 0)
            {
                return SessionOperationResult.Greska("Maksimalan broj učesnika mora biti veći od 0.");
            }

            if (podaci.Trajanje <= 0)
            {
                return SessionOperationResult.Greska("Trajanje sesije mora biti veće od 0.");
            }

            var postojiPredmet = await _context.Predmeti
                .AnyAsync(p => p.IdPredmeta == podaci.PredmetId);

            if (!postojiPredmet)
            {
                return SessionOperationResult.Greska("Odabrani predmet ne postoji.");
            }

            var postojiLokacija = await _context.Lokacije
                .AnyAsync(l => l.IdLokacije == podaci.LokacijaId);

            if (!postojiLokacija)
            {
                return SessionOperationResult.Greska("Odabrana lokacija ne postoji.");
            }

            var sesija = new SesijaUcenjaBuilder()
                .PostaviNaziv(podaci.Naziv)
                .PostaviOpis(podaci.Opis)
                .PostaviDatumVrijeme(podaci.DatumVrijeme)
                .PostaviTrajanje(podaci.Trajanje)
                .PostaviPredmet(podaci.PredmetId)
                .PostaviLokaciju(podaci.LokacijaId)
                .PostaviKreatora(kreator.Id)
                .PostaviMaksimalanBrojUcesnika(podaci.MaksimalanBrojUcesnika)
                .PostaviStatus(podaci.StatusSesije)
                .Kreiraj();

            _context.SesijeUcenja.Add(sesija);
            await _context.SaveChangesAsync();

            return SessionOperationResult.Uspjeh("Sesija je uspješno kreirana.");
        }

        public async Task<SessionOperationResult> PrijaviKorisnikaNaSesijuAsync(
            int sesijaId,
            Korisnik korisnik)
        {
            if (korisnik == null)
            {
                return SessionOperationResult.Greska("Korisnik nije pronađen.");
            }

            var sesija = await _context.SesijeUcenja
                .FirstOrDefaultAsync(s => s.IdSesije == sesijaId);

            if (sesija == null)
            {
                return SessionOperationResult.Greska("Sesija nije pronađena.");
            }

            if (sesija.StatusSesije != StatusSesije.Aktivna)
            {
                return SessionOperationResult.Greska("Prijava nije moguća jer sesija nije aktivna.");
            }

            if (sesija.BrojSlobodnihMjesta <= 0)
            {
                return SessionOperationResult.Greska("Prijava nije moguća jer nema slobodnih mjesta.");
            }

            var vecPrijavljen = await _context.PrijaveNaSesije
                .AnyAsync(p =>
                    p.SesijaId == sesijaId &&
                    p.KorisnikId == korisnik.Id &&
                    p.StatusPrijave == StatusPrijave.Prijavljen);

            if (vecPrijavljen)
            {
                return SessionOperationResult.Greska("Već ste prijavljeni na ovu sesiju.");
            }

            var prijava = new PrijavaNaSesiju
            {
                DatumPrijave = DateTime.UtcNow,
                KorisnikId = korisnik.Id,
                SesijaId = sesija.IdSesije,
                StatusPrijave = StatusPrijave.Prijavljen
            };

            sesija.BrojSlobodnihMjesta--;

            var obavjestenje = _obavjestenjeFactory.KreirajObavjestenje(
                TipObavjestenja.Prijava,
                korisnik,
                sesija);

            obavjestenje.Korisnik = korisnik;

            _context.PrijaveNaSesije.Add(prijava);
            _context.Obavjestenja.Add(obavjestenje);

            await _obavjestenjeSender.PosaljiAsync(obavjestenje);
            await _context.SaveChangesAsync();

            return SessionOperationResult.Uspjeh("Uspješno ste se prijavili na sesiju.");
        }

        public async Task<(SessionOperationResult Rezultat, int? SesijaId)> OtkaziPrijavuAsync(
            int prijavaId,
            Korisnik korisnik)
        {
            if (korisnik == null)
            {
                return (SessionOperationResult.Greska("Korisnik nije pronađen."), null);
            }

            var prijava = await _context.PrijaveNaSesije
                .Include(p => p.SesijaUcenja)
                .FirstOrDefaultAsync(p =>
                    p.IdPrijave == prijavaId &&
                    p.KorisnikId == korisnik.Id &&
                    p.StatusPrijave == StatusPrijave.Prijavljen);

            if (prijava == null)
            {
                return (SessionOperationResult.Greska("Aktivna prijava nije pronađena."), null);
            }

            if (prijava.SesijaUcenja == null)
            {
                return (SessionOperationResult.Greska("Sesija za ovu prijavu nije pronađena."), null);
            }

            prijava.StatusPrijave = StatusPrijave.Otkazan;
            prijava.SesijaUcenja.BrojSlobodnihMjesta++;

            var obavjestenje = _obavjestenjeFactory.KreirajObavjestenje(
                TipObavjestenja.Otkazivanje,
                korisnik,
                prijava.SesijaUcenja);

            obavjestenje.Korisnik = korisnik;

            _context.Obavjestenja.Add(obavjestenje);

            await _obavjestenjeSender.PosaljiAsync(obavjestenje);
            await _context.SaveChangesAsync();

            return (
                SessionOperationResult.Uspjeh("Uspješno ste otkazali prijavu."),
                prijava.SesijaId
            );
        }
    }
}
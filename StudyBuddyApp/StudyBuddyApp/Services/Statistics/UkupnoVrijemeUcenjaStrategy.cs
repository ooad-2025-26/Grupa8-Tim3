using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Services.Statistics
{
    public class UkupnoVrijemeUcenjaStrategy : IStatistikaStrategy
    {
        private readonly ApplicationDbContext _context;

        public UkupnoVrijemeUcenjaStrategy(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StatistikaRezultat> IzracunajAsync(Korisnik korisnik)
        {
            var ukupnoMinuta = await _context.Prisustva
                .Where(p => p.KorisnikId == korisnik.Id)
                .SumAsync(p => p.TrajanjeUcenja);

            var sati = ukupnoMinuta / 60;
            var minute = ukupnoMinuta % 60;

            return new StatistikaRezultat
            {
                Naziv = "Ukupno vrijeme učenja",
                Vrijednost = $"{sati}h {minute}min",
                Opis = "Ukupno evidentirano vrijeme učenja kroz sesije."
            };
        }
    }
}
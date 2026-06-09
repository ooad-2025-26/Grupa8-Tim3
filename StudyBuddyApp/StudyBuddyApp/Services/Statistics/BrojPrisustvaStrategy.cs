using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Services.Statistics
{
    public class BrojPrisustavaStrategy : IStatistikaStrategy
    {
        private readonly ApplicationDbContext _context;

        public BrojPrisustavaStrategy(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StatistikaRezultat> IzracunajAsync(Korisnik korisnik)
        {
            var brojPrisustava = await _context.Prisustva
                .CountAsync(p => p.KorisnikId == korisnik.Id);

            return new StatistikaRezultat
            {
                Naziv = "Evidentirana prisustva",
                Vrijednost = brojPrisustava.ToString(),
                Opis = "Broj sesija na kojima je evidentirano vaše prisustvo."
            };
        }
    }
}
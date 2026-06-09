using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Services.Statistics
{
    public class BrojSesijaStrategy : IStatistikaStrategy
    {
        private readonly ApplicationDbContext _context;

        public BrojSesijaStrategy(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StatistikaRezultat> IzracunajAsync(Korisnik korisnik)
        {
            var brojSesija = await _context.SesijeUcenja
                .CountAsync(s => s.KreatorId == korisnik.Id);

            return new StatistikaRezultat
            {
                Naziv = "Kreirane sesije",
                Vrijednost = brojSesija.ToString(),
                Opis = "Ukupan broj sesija učenja koje ste kreirali."
            };
        }
    }
}
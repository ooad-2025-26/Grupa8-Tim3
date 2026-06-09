using StudyBuddyApp.Models;

namespace StudyBuddyApp.Services.Statistics
{
    public interface IStatistikaStrategy
    {
        Task<StatistikaRezultat> IzracunajAsync(Korisnik korisnik);
    }
}
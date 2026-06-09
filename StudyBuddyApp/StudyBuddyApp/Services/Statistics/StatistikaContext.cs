using StudyBuddyApp.Models;

namespace StudyBuddyApp.Services.Statistics
{
    public class StatistikaContext
    {
        private readonly IEnumerable<IStatistikaStrategy> _strategije;

        public StatistikaContext(IEnumerable<IStatistikaStrategy> strategije)
        {
            _strategije = strategije;
        }

        public async Task<List<StatistikaRezultat>> IzracunajSveAsync(Korisnik korisnik)
        {
            var rezultati = new List<StatistikaRezultat>();

            foreach (var strategija in _strategije)
            {
                rezultati.Add(await strategija.IzracunajAsync(korisnik));
            }

            return rezultati;
        }
    }
}
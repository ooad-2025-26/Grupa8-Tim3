using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyApp.Models
{
    public class Prisustvo
    {
        public Prisustvo() { }

        [Key]
        public int IdPrisustva { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }

        public Korisnik? Korisnik { get; set; }

        [ForeignKey("SesijaUcenja")]
        public int SesijaId { get; set; }

        public SesijaUcenja? SesijaUcenja { get; set; }

        public int TrajanjeUcenja { get; set; }

        public StatusPrisustva StatusPrisustva { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyApp.Models
{
    public class PrijavaNaSesiju
    {
        public PrijavaNaSesiju() { }

        [Key]
        public int IdPrijave { get; set; }

        public DateTime DatumPrijave { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }

        public Korisnik? Korisnik { get; set; }

        [ForeignKey("SesijaUcenja")]
        public int SesijaId { get; set; }

        public SesijaUcenja? SesijaUcenja { get; set; }

        public StatusPrijave StatusPrijave { get; set; }
    }
}
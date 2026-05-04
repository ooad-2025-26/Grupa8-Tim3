using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyApp.Models
{
    public class Obavjestenje
    {
        public Obavjestenje() { }

        [Key]
        public int IdObavjestenja { get; set; }

        public string Naslov { get; set; } = string.Empty;

        public string Sadrzaj { get; set; } = string.Empty;

        public DateTime DatumSlanja { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }

        public Korisnik? Korisnik { get; set; }

        public TipObavjestenja TipObavjestenja { get; set; }

        public bool Procitano { get; set; }
    }
}
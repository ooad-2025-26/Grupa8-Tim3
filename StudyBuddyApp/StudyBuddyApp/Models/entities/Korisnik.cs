using System.ComponentModel.DataAnnotations;

namespace StudyBuddyApp.Models
{
    public class Korisnik
    {
        public Korisnik() { }

        [Key]
        public int IdKorisnika { get; set; }

        public string Ime { get; set; } = string.Empty;

        public string Prezime { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Lozinka { get; set; } = string.Empty;

        public TipUloge UlogaId { get; set; }

        public StatusNaloga StatusNaloga { get; set; }
    }
}
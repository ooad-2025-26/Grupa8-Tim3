using Microsoft.AspNetCore.Identity;

namespace StudyBuddyApp.Models
{
    public class Korisnik : IdentityUser
    {
        public Korisnik() { }

        public string Ime { get; set; } = string.Empty;

        public string Prezime { get; set; } = string.Empty;

        public TipUloge Uloga { get; set; }

        public StatusNaloga StatusNaloga { get; set; }
    }
}
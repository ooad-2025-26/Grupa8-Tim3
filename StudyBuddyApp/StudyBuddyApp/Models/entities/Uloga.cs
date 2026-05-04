using System.ComponentModel.DataAnnotations;

namespace StudyBuddyApp.Models
{
    public class Uloga
    {
        public Uloga() { }

        [Key]
        public int IdUloge { get; set; }

        public TipUloge Naziv { get; set; }

        public string Opis { get; set; } = string.Empty;
    }
}
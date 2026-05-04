using System.ComponentModel.DataAnnotations;

namespace StudyBuddyApp.Models
{
    public class Predmet
    {
        public Predmet() { }

        [Key]
        public int IdPredmeta { get; set; }

        public string Naziv { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;

        public string Oznaka { get; set; } = string.Empty;

        public StatusPredmeta StatusPredmeta { get; set; }
    }
}
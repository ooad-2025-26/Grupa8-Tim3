using System.ComponentModel.DataAnnotations;

namespace StudyBuddyApp.Models
{
    public class Lokacija
    {
        public Lokacija() { }

        [Key]
        public int IdLokacije { get; set; }

        public string Naziv { get; set; } = string.Empty;

        public TipLokacije TipLokacije { get; set; }

        public string Adresa { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;
    }
}
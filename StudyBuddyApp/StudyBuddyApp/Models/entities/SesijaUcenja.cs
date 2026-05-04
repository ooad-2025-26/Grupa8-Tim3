using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyApp.Models
{
    public class SesijaUcenja
    {
        public SesijaUcenja() { }

        [Key]
        public int IdSesije { get; set; }

        public string Naziv { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;

        public DateTime DatumVrijeme { get; set; }

        public int Trajanje { get; set; }

        [ForeignKey("Lokacija")]
        public int LokacijaId { get; set; }

        public Lokacija? Lokacija { get; set; }

        [ForeignKey("Predmet")]
        public int PredmetId { get; set; }

        public Predmet? Predmet { get; set; }

        [ForeignKey("Kreator")]
        public int KreatorId { get; set; }

        public Korisnik? Kreator { get; set; }

        public int MaksimalanBrojUcesnika { get; set; }

        public int BrojSlobodnihMjesta { get; set; }

        public StatusSesije StatusSesije { get; set; }
    }
}
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Services.Observers
{
    public class SesijaPromjenaInfo
    {
        public int SesijaId { get; set; }

        public DateTime StariDatumVrijeme { get; set; }
        public DateTime NoviDatumVrijeme { get; set; }

        public int StaraLokacijaId { get; set; }
        public int NovaLokacijaId { get; set; }

        public StatusSesije StariStatus { get; set; }
        public StatusSesije NoviStatus { get; set; }

        public bool DatumPromijenjen => StariDatumVrijeme != NoviDatumVrijeme;
        public bool LokacijaPromijenjena => StaraLokacijaId != NovaLokacijaId;
        public bool StatusPromijenjen => StariStatus != NoviStatus;

        public bool ImaPromjena =>
            DatumPromijenjen || LokacijaPromijenjena || StatusPromijenjen;
    }
}
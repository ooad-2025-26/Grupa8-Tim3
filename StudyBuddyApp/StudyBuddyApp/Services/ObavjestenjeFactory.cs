using StudyBuddyApp.Models;

namespace StudyBuddyApp.Services
{
    public class ObavjestenjeFactory
    {
        public Obavjestenje KreirajObavjestenje(
            TipObavjestenja tip,
            Korisnik korisnik,
            SesijaUcenja sesija)
        {
            return tip switch
            {
                TipObavjestenja.Prijava => new Obavjestenje
                {
                    Naslov = "Prijava na sesiju",
                    Sadrzaj = $"Uspješno ste prijavljeni na sesiju: {sesija.Naziv}.",
                    DatumSlanja = DateTime.UtcNow,
                    KorisnikId = korisnik.Id,
                    TipObavjestenja = TipObavjestenja.Prijava,
                    Procitano = false
                },

                TipObavjestenja.Otkazivanje => new Obavjestenje
                {
                    Naslov = "Otkazivanje prijave",
                    Sadrzaj = $"Vaša prijava na sesiju {sesija.Naziv} je otkazana.",
                    DatumSlanja = DateTime.UtcNow,
                    KorisnikId = korisnik.Id,
                    TipObavjestenja = TipObavjestenja.Otkazivanje,
                    Procitano = false
                },

                TipObavjestenja.Podsjetnik => new Obavjestenje
                {
                    Naslov = "Podsjetnik za sesiju",
                    Sadrzaj = $"Podsjetnik: sesija {sesija.Naziv} počinje {sesija.DatumVrijeme}.",
                    DatumSlanja = DateTime.UtcNow,
                    KorisnikId = korisnik.Id,
                    TipObavjestenja = TipObavjestenja.Podsjetnik,
                    Procitano = false
                },

                TipObavjestenja.PromjenaTermina => new Obavjestenje
                {
                    Naslov = "Promjena termina sesije",
                    Sadrzaj = $"Termin sesije {sesija.Naziv} je promijenjen.",
                    DatumSlanja = DateTime.UtcNow,
                    KorisnikId = korisnik.Id,
                    TipObavjestenja = TipObavjestenja.PromjenaTermina,
                    Procitano = false
                },

                _ => new Obavjestenje
                {
                    Naslov = "Obavještenje",
                    Sadrzaj = $"Novo obavještenje za sesiju: {sesija.Naziv}.",
                    DatumSlanja = DateTime.UtcNow,
                    KorisnikId = korisnik.Id,
                    TipObavjestenja = tip,
                    Procitano = false
                }
            };
        }
    }
}
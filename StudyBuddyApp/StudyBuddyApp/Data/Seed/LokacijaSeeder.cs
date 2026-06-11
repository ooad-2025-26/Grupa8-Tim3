using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Data.Seed
{
    public static class LokacijaSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext db)
        {
            var lokacije = new List<Lokacija>
            {
                new Lokacija
                {
                    Naziv = "Homework Hub",
                    TipLokacije = TipLokacije.Fizicka,
                    Adresa = "Grbavička bb, Sarajevo",
                    Link = string.Empty
                },
                new Lokacija
                {
                    Naziv = "Čitaonica STELEKS",
                    TipLokacije = TipLokacije.Fizicka,
                    Adresa = "Zmaja od Bosne bb, Sarajevo",
                    Link = string.Empty
                },
                new Lokacija
                {
                    Naziv = "Campus Lounge",
                    TipLokacije = TipLokacije.Fizicka,
                    Adresa = "Zmaja od Bosne bb, Sarajevo",
                    Link = string.Empty
                },
                new Lokacija
                {
                    Naziv = "Čitaonica \"Safet Zajko\"",
                    TipLokacije = TipLokacije.Fizicka,
                    Adresa = "Halilovići bb, Sarajevo",
                    Link = string.Empty
                },
                new Lokacija
                {
                    Naziv = "Gazi Husrev-begova biblioteka",
                    TipLokacije = TipLokacije.Fizicka,
                    Adresa = "Gazi Husrev-begova 46, Sarajevo",
                    Link = string.Empty
                },
                new Lokacija
                {
                    Naziv = "Online StudyBuddy soba 1",
                    TipLokacije = TipLokacije.Online,
                    Adresa = string.Empty,
                    Link = "https://meet.google.com/new"
                },
                new Lokacija
                {
                    Naziv = "Online StudyBuddy soba 2",
                    TipLokacije = TipLokacije.Online,
                    Adresa = string.Empty,
                    Link = "https://meet.google.com/new"
                },
                new Lokacija
                {
                    Naziv = "Online StudyBuddy soba 3",
                    TipLokacije = TipLokacije.Online,
                    Adresa = string.Empty,
                    Link = "https://meet.google.com/new"
                },
                new Lokacija
                {
                    Naziv = "Online StudyBuddy soba 4",
                    TipLokacije = TipLokacije.Online,
                    Adresa = string.Empty,
                    Link = "https://meet.google.com/new"
                },
                new Lokacija
                {
                    Naziv = "Online StudyBuddy soba 5",
                    TipLokacije = TipLokacije.Online,
                    Adresa = string.Empty,
                    Link = "https://meet.google.com/new"
                }
            };

            foreach (var lokacija in lokacije)
            {
                bool postoji = await db.Lokacije.AnyAsync(l =>
                    l.Naziv == lokacija.Naziv &&
                    l.TipLokacije == lokacija.TipLokacije);

                if (!postoji)
                {
                    db.Lokacije.Add(lokacija);
                }
            }

            await db.SaveChangesAsync();
        }
    }
}
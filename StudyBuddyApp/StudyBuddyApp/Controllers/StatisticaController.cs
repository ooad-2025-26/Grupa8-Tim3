using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Models;
using StudyBuddyApp.Services.Statistics;

namespace StudyBuddyApp.Controllers
{
    [Authorize(Roles = "Administrator,Student")]
    public class StatistikaController : Controller
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly StatistikaContext _statistikaContext;

        public StatistikaController(
            UserManager<Korisnik> userManager,
            StatistikaContext statistikaContext)
        {
            _userManager = userManager;
            _statistikaContext = statistikaContext;
        }

        public async Task<IActionResult> Index(string? korisnikId)
        {
            var trenutniKorisnik = await _userManager.GetUserAsync(User);

            if (trenutniKorisnik == null)
            {
                return Challenge();
            }

            Korisnik korisnikZaStatistiku = trenutniKorisnik;

            if (User.IsInRole("Administrator"))
            {
                var korisnici = await _userManager.Users
                    .OrderBy(k => k.Ime)
                    .ThenBy(k => k.Prezime)
                    .Select(k => new
                    {
                        k.Id,
                        Naziv = k.Ime + " " + k.Prezime + " (" + k.Email + ")"
                    })
                    .ToListAsync();

                ViewBag.Korisnici = new SelectList(
                    korisnici,
                    "Id",
                    "Naziv",
                    string.IsNullOrWhiteSpace(korisnikId) ? trenutniKorisnik.Id : korisnikId
                );

                if (!string.IsNullOrWhiteSpace(korisnikId))
                {
                    var odabraniKorisnik = await _userManager.FindByIdAsync(korisnikId);

                    if (odabraniKorisnik == null)
                    {
                        TempData["Greska"] = "Odabrani korisnik nije pronađen.";
                    }
                    else
                    {
                        korisnikZaStatistiku = odabraniKorisnik;
                    }
                }
            }

            ViewBag.PrikazaniKorisnik = korisnikZaStatistiku;

            var rezultati = await _statistikaContext.IzracunajSveAsync(korisnikZaStatistiku);

            return View(rezultati);
        }
    }
}
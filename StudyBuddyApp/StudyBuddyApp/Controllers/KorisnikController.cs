using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class KorisnikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KorisnikController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ime,Prezime,Email,Uloga,StatusNaloga")] Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                korisnik.UserName = korisnik.Email;
                korisnik.NormalizedUserName = korisnik.Email?.ToUpper();
                korisnik.NormalizedEmail = korisnik.Email?.ToUpper();
                korisnik.EmailConfirmed = true;

                _context.Users.Add(korisnik);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(korisnik);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Users.FindAsync(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Ime,Prezime,Email,Uloga,StatusNaloga")] Korisnik korisnik)
        {
            if (id != korisnik.Id)
            {
                return NotFound();
            }

            var postojeciKorisnik = await _context.Users.FindAsync(id);

            if (postojeciKorisnik == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    postojeciKorisnik.Ime = korisnik.Ime;
                    postojeciKorisnik.Prezime = korisnik.Prezime;
                    postojeciKorisnik.Email = korisnik.Email;
                    postojeciKorisnik.UserName = korisnik.Email;
                    postojeciKorisnik.NormalizedEmail = korisnik.Email?.ToUpper();
                    postojeciKorisnik.NormalizedUserName = korisnik.Email?.ToUpper();
                    postojeciKorisnik.Uloga = korisnik.Uloga;
                    postojeciKorisnik.StatusNaloga = korisnik.StatusNaloga;

                    _context.Update(postojeciKorisnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisnikExists(korisnik.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(korisnik);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var korisnik = await _context.Users.FindAsync(id);

            if (korisnik != null)
            {
                _context.Users.Remove(korisnik);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KorisnikExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
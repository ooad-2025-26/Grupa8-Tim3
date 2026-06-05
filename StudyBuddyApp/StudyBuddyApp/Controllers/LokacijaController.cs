using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Controllers
{
    [Authorize]
    public class LokacijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LokacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Lokacije.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lokacija = await _context.Lokacije
                .FirstOrDefaultAsync(m => m.IdLokacije == id);

            if (lokacija == null)
            {
                return NotFound();
            }

            return View(lokacija);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLokacije,Naziv,TipLokacije,Adresa,Link")] Lokacija lokacija)
        {
            ValidirajLokaciju(lokacija);

            if (ModelState.IsValid)
            {
                _context.Lokacije.Add(lokacija);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(lokacija);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lokacija = await _context.Lokacije.FindAsync(id);

            if (lokacija == null)
            {
                return NotFound();
            }

            return View(lokacija);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLokacije,Naziv,TipLokacije,Adresa,Link")] Lokacija lokacija)
        {
            if (id != lokacija.IdLokacije)
            {
                return NotFound();
            }

            ValidirajLokaciju(lokacija);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lokacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LokacijaExists(lokacija.IdLokacije))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(lokacija);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lokacija = await _context.Lokacije
                .FirstOrDefaultAsync(m => m.IdLokacije == id);

            if (lokacija == null)
            {
                return NotFound();
            }

            return View(lokacija);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lokacija = await _context.Lokacije.FindAsync(id);

            if (lokacija != null)
            {
                _context.Lokacije.Remove(lokacija);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void ValidirajLokaciju(Lokacija lokacija)
        {
            if (lokacija.TipLokacije == TipLokacije.Online)
            {
                ModelState.Remove("Adresa");
                lokacija.Adresa = string.Empty;

                if (string.IsNullOrWhiteSpace(lokacija.Link))
                {
                    ModelState.AddModelError("Link", "Za online lokaciju potrebno je unijeti link.");
                }
            }

            if (lokacija.TipLokacije == TipLokacije.Fizicka)
            {
                ModelState.Remove("Link");
                lokacija.Link = string.Empty;

                if (string.IsNullOrWhiteSpace(lokacija.Adresa))
                {
                    ModelState.AddModelError("Adresa", "Za fizičku lokaciju potrebno je unijeti adresu.");
                }
            }
        }

        private bool LokacijaExists(int id)
        {
            return _context.Lokacije.Any(e => e.IdLokacije == id);
        }
    }
}
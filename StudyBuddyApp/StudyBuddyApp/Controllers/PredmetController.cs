using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Controllers
{
    [Authorize]
    public class PredmetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PredmetController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? pretraga,
            GodinaStudija? godinaStudija,
            SmjerStudija? smjerStudija,
            StatusPredmeta? statusPredmeta)
        {
            var predmeti = _context.Predmeti.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pretraga))
            {
                var pojam = pretraga.ToLower();

                predmeti = predmeti.Where(p =>
                    p.Naziv.ToLower().Contains(pojam) ||
                    p.Opis.ToLower().Contains(pojam) ||
                    p.Oznaka.ToLower().Contains(pojam));
            }

            if (godinaStudija.HasValue)
            {
                predmeti = predmeti.Where(p => p.GodinaStudija == godinaStudija.Value);
            }

            if (smjerStudija.HasValue)
            {
                predmeti = predmeti.Where(p => p.SmjerStudija == smjerStudija.Value);
            }

            if (statusPredmeta.HasValue)
            {
                predmeti = predmeti.Where(p => p.StatusPredmeta == statusPredmeta.Value);
            }

            ViewBag.Pretraga = pretraga;
            ViewBag.GodinaStudija = godinaStudija?.ToString();
            ViewBag.SmjerStudija = smjerStudija?.ToString();
            ViewBag.StatusPredmeta = statusPredmeta?.ToString();

            return View(await predmeti
                .OrderBy(p => p.GodinaStudija)
                .ThenBy(p => p.SmjerStudija)
                .ThenBy(p => p.Naziv)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predmet = await _context.Predmeti
                .FirstOrDefaultAsync(m => m.IdPredmeta == id);

            if (predmet == null)
            {
                return NotFound();
            }

            return View(predmet);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPredmeta,Naziv,Oznaka,GodinaStudija,SmjerStudija,StatusPredmeta")] Predmet predmet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(predmet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(predmet);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predmet = await _context.Predmeti.FindAsync(id);

            if (predmet == null)
            {
                return NotFound();
            }

            return View(predmet);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPredmeta,Naziv,Oznaka,GodinaStudija,SmjerStudija,StatusPredmeta")] Predmet predmet)
        {
            if (id != predmet.IdPredmeta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(predmet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PredmetExists(predmet.IdPredmeta))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(predmet);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predmet = await _context.Predmeti
                .FirstOrDefaultAsync(m => m.IdPredmeta == id);

            if (predmet == null)
            {
                return NotFound();
            }

            return View(predmet);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var predmet = await _context.Predmeti.FindAsync(id);

            if (predmet != null)
            {
                _context.Predmeti.Remove(predmet);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PredmetExists(int id)
        {
            return _context.Predmeti.Any(e => e.IdPredmeta == id);
        }
    }
}
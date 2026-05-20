using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Controllers
{
    [Authorize]
    public class ObavjestenjeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ObavjestenjeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var obavjestenja = _context.Obavjestenja
                .Include(o => o.Korisnik);

            return View(await obavjestenja.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obavjestenje = await _context.Obavjestenja
                .Include(o => o.Korisnik)
                .FirstOrDefaultAsync(m => m.IdObavjestenja == id);

            if (obavjestenje == null)
            {
                return NotFound();
            }

            return View(obavjestenje);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        public IActionResult Create()
        {
            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Ime");

            return View();
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdObavjestenja,Naslov,Sadrzaj,DatumSlanja,KorisnikId,TipObavjestenja,Procitano")] Obavjestenje obavjestenje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(obavjestenje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Ime", obavjestenje.KorisnikId);

            return View(obavjestenje);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obavjestenje = await _context.Obavjestenja.FindAsync(id);

            if (obavjestenje == null)
            {
                return NotFound();
            }

            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Ime", obavjestenje.KorisnikId);

            return View(obavjestenje);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdObavjestenja,Naslov,Sadrzaj,DatumSlanja,KorisnikId,TipObavjestenja,Procitano")] Obavjestenje obavjestenje)
        {
            if (id != obavjestenje.IdObavjestenja)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(obavjestenje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObavjestenjeExists(obavjestenje.IdObavjestenja))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Ime", obavjestenje.KorisnikId);

            return View(obavjestenje);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obavjestenje = await _context.Obavjestenja
                .Include(o => o.Korisnik)
                .FirstOrDefaultAsync(m => m.IdObavjestenja == id);

            if (obavjestenje == null)
            {
                return NotFound();
            }

            return View(obavjestenje);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obavjestenje = await _context.Obavjestenja.FindAsync(id);

            if (obavjestenje != null)
            {
                _context.Obavjestenja.Remove(obavjestenje);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ObavjestenjeExists(int id)
        {
            return _context.Obavjestenja.Any(e => e.IdObavjestenja == id);
        }
    }
}
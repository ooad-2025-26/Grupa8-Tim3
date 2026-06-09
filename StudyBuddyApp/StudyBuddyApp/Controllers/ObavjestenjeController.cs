using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<Korisnik> _userManager;

        public ObavjestenjeController(
            ApplicationDbContext context,
            UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik == null)
            {
                return Challenge();
            }

            IQueryable<Obavjestenje> obavjestenja = _context.Obavjestenja
                .Include(o => o.Korisnik);

            if (!User.IsInRole("Administrator") && !User.IsInRole("Moderator"))
            {
                obavjestenja = obavjestenja.Where(o => o.KorisnikId == korisnik.Id);
            }

            return View(await obavjestenja
                .OrderByDescending(o => o.DatumSlanja)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik == null)
            {
                return Challenge();
            }

            var obavjestenje = await _context.Obavjestenja
                .Include(o => o.Korisnik)
                .FirstOrDefaultAsync(m => m.IdObavjestenja == id);

            if (obavjestenje == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Administrator") &&
                !User.IsInRole("Moderator") &&
                obavjestenje.KorisnikId != korisnik.Id)
            {
                return Forbid();
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
            if (obavjestenje.DatumSlanja == default)
            {
                obavjestenje.DatumSlanja = DateTime.UtcNow;
            }

            if (ModelState.IsValid)
            {
                _context.Obavjestenja.Add(obavjestenje);
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

            if (obavjestenje == null)
            {
                return NotFound();
            }

            _context.Obavjestenja.Remove(obavjestenje);
            await _context.SaveChangesAsync();

            TempData["Poruka"] = "Obavještenje je uspješno obrisano.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OznaciKaoProcitano(int id)
        {
            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik == null)
            {
                return Challenge();
            }

            var obavjestenje = await _context.Obavjestenja.FindAsync(id);

            if (obavjestenje == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Administrator") &&
                !User.IsInRole("Moderator") &&
                obavjestenje.KorisnikId != korisnik.Id)
            {
                return Forbid();
            }

            obavjestenje.Procitano = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFromDetails(int id)
        {
            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik == null)
            {
                return Challenge();
            }

            var obavjestenje = await _context.Obavjestenja
                .FirstOrDefaultAsync(o => o.IdObavjestenja == id);

            if (obavjestenje == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Administrator") &&
                !User.IsInRole("Moderator") &&
                obavjestenje.KorisnikId != korisnik.Id)
            {
                return Forbid();
            }

            _context.Obavjestenja.Remove(obavjestenje);
            await _context.SaveChangesAsync();

            TempData["Poruka"] = "Obavještenje je uspješno obrisano.";

            return RedirectToAction(nameof(Index));
        }

        private bool ObavjestenjeExists(int id)
        {
            return _context.Obavjestenja.Any(e => e.IdObavjestenja == id);
        }
    }
}
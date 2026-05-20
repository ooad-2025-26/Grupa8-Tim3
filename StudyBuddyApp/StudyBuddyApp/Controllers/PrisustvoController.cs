using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Controllers
{
    [Authorize]
    public class PrisustvoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrisustvoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var prisustva = _context.Prisustva
                .Include(p => p.Korisnik)
                .Include(p => p.SesijaUcenja);

            return View(await prisustva.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prisustvo = await _context.Prisustva
                .Include(p => p.Korisnik)
                .Include(p => p.SesijaUcenja)
                .FirstOrDefaultAsync(m => m.IdPrisustva == id);

            if (prisustvo == null)
            {
                return NotFound();
            }

            return View(prisustvo);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        public IActionResult Create()
        {
            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Ime");
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "Naziv");

            return View();
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPrisustva,KorisnikId,SesijaId,TrajanjeUcenja,StatusPrisustva")] Prisustvo prisustvo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prisustvo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Ime", prisustvo.KorisnikId);
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "Naziv", prisustvo.SesijaId);

            return View(prisustvo);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prisustvo = await _context.Prisustva.FindAsync(id);

            if (prisustvo == null)
            {
                return NotFound();
            }

            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Ime", prisustvo.KorisnikId);
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "Naziv", prisustvo.SesijaId);

            return View(prisustvo);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPrisustva,KorisnikId,SesijaId,TrajanjeUcenja,StatusPrisustva")] Prisustvo prisustvo)
        {
            if (id != prisustvo.IdPrisustva)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prisustvo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrisustvoExists(prisustvo.IdPrisustva))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Ime", prisustvo.KorisnikId);
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "Naziv", prisustvo.SesijaId);

            return View(prisustvo);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prisustvo = await _context.Prisustva
                .Include(p => p.Korisnik)
                .Include(p => p.SesijaUcenja)
                .FirstOrDefaultAsync(m => m.IdPrisustva == id);

            if (prisustvo == null)
            {
                return NotFound();
            }

            return View(prisustvo);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prisustvo = await _context.Prisustva.FindAsync(id);

            if (prisustvo != null)
            {
                _context.Prisustva.Remove(prisustvo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PrisustvoExists(int id)
        {
            return _context.Prisustva.Any(e => e.IdPrisustva == id);
        }
    }
}
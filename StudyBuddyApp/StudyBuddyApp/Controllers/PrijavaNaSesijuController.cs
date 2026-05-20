using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Controllers
{
    [Authorize]
    public class PrijavaNaSesijuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrijavaNaSesijuController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var prijave = _context.PrijaveNaSesije
                .Include(p => p.Korisnik)
                .Include(p => p.SesijaUcenja);

            return View(await prijave.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavaNaSesiju = await _context.PrijaveNaSesije
                .Include(p => p.Korisnik)
                .Include(p => p.SesijaUcenja)
                .FirstOrDefaultAsync(m => m.IdPrijave == id);

            if (prijavaNaSesiju == null)
            {
                return NotFound();
            }

            return View(prijavaNaSesiju);
        }

        [Authorize(Roles = "Administrator,Student")]
        public IActionResult Create()
        {
            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Ime");
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "Naziv");

            return View();
        }

        [Authorize(Roles = "Administrator,Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPrijave,DatumPrijave,KorisnikId,SesijaId,StatusPrijave")] PrijavaNaSesiju prijavaNaSesiju)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prijavaNaSesiju);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Ime", prijavaNaSesiju.KorisnikId);
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "Naziv", prijavaNaSesiju.SesijaId);

            return View(prijavaNaSesiju);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavaNaSesiju = await _context.PrijaveNaSesije.FindAsync(id);

            if (prijavaNaSesiju == null)
            {
                return NotFound();
            }

            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Ime", prijavaNaSesiju.KorisnikId);
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "Naziv", prijavaNaSesiju.SesijaId);

            return View(prijavaNaSesiju);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPrijave,DatumPrijave,KorisnikId,SesijaId,StatusPrijave")] PrijavaNaSesiju prijavaNaSesiju)
        {
            if (id != prijavaNaSesiju.IdPrijave)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prijavaNaSesiju);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrijavaNaSesijuExists(prijavaNaSesiju.IdPrijave))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Ime", prijavaNaSesiju.KorisnikId);
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "Naziv", prijavaNaSesiju.SesijaId);

            return View(prijavaNaSesiju);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavaNaSesiju = await _context.PrijaveNaSesije
                .Include(p => p.Korisnik)
                .Include(p => p.SesijaUcenja)
                .FirstOrDefaultAsync(m => m.IdPrijave == id);

            if (prijavaNaSesiju == null)
            {
                return NotFound();
            }

            return View(prijavaNaSesiju);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prijavaNaSesiju = await _context.PrijaveNaSesije.FindAsync(id);

            if (prijavaNaSesiju != null)
            {
                _context.PrijaveNaSesije.Remove(prijavaNaSesiju);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PrijavaNaSesijuExists(int id)
        {
            return _context.PrijaveNaSesije.Any(e => e.IdPrijave == id);
        }
    }
}
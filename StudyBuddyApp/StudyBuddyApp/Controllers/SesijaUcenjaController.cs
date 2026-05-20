using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Controllers
{
    [Authorize]
    public class SesijaUcenjaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SesijaUcenjaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sesije = _context.SesijeUcenja
                .Include(s => s.Kreator)
                .Include(s => s.Lokacija)
                .Include(s => s.Predmet);

            return View(await sesije.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sesijaUcenja = await _context.SesijeUcenja
                .Include(s => s.Kreator)
                .Include(s => s.Lokacija)
                .Include(s => s.Predmet)
                .FirstOrDefaultAsync(m => m.IdSesije == id);

            if (sesijaUcenja == null)
            {
                return NotFound();
            }

            return View(sesijaUcenja);
        }

        [Authorize(Roles = "Administrator,Student")]
        public IActionResult Create()
        {
            ViewData["KreatorId"] = new SelectList(_context.Users, "Id", "Ime");
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "IdLokacije", "Naziv");
            ViewData["PredmetId"] = new SelectList(_context.Predmeti, "IdPredmeta", "Naziv");

            return View();
        }

        [Authorize(Roles = "Administrator,Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSesije,Naziv,Opis,DatumVrijeme,Trajanje,LokacijaId,PredmetId,KreatorId,MaksimalanBrojUcesnika,BrojSlobodnihMjesta,StatusSesije")] SesijaUcenja sesijaUcenja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sesijaUcenja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["KreatorId"] = new SelectList(_context.Users, "Id", "Ime", sesijaUcenja.KreatorId);
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "IdLokacije", "Naziv", sesijaUcenja.LokacijaId);
            ViewData["PredmetId"] = new SelectList(_context.Predmeti, "IdPredmeta", "Naziv", sesijaUcenja.PredmetId);

            return View(sesijaUcenja);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sesijaUcenja = await _context.SesijeUcenja.FindAsync(id);

            if (sesijaUcenja == null)
            {
                return NotFound();
            }

            ViewData["KreatorId"] = new SelectList(_context.Users, "Id", "Ime", sesijaUcenja.KreatorId);
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "IdLokacije", "Naziv", sesijaUcenja.LokacijaId);
            ViewData["PredmetId"] = new SelectList(_context.Predmeti, "IdPredmeta", "Naziv", sesijaUcenja.PredmetId);

            return View(sesijaUcenja);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSesije,Naziv,Opis,DatumVrijeme,Trajanje,LokacijaId,PredmetId,KreatorId,MaksimalanBrojUcesnika,BrojSlobodnihMjesta,StatusSesije")] SesijaUcenja sesijaUcenja)
        {
            if (id != sesijaUcenja.IdSesije)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sesijaUcenja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SesijaUcenjaExists(sesijaUcenja.IdSesije))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["KreatorId"] = new SelectList(_context.Users, "Id", "Ime", sesijaUcenja.KreatorId);
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "IdLokacije", "Naziv", sesijaUcenja.LokacijaId);
            ViewData["PredmetId"] = new SelectList(_context.Predmeti, "IdPredmeta", "Naziv", sesijaUcenja.PredmetId);

            return View(sesijaUcenja);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sesijaUcenja = await _context.SesijeUcenja
                .Include(s => s.Kreator)
                .Include(s => s.Lokacija)
                .Include(s => s.Predmet)
                .FirstOrDefaultAsync(m => m.IdSesije == id);

            if (sesijaUcenja == null)
            {
                return NotFound();
            }

            return View(sesijaUcenja);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sesijaUcenja = await _context.SesijeUcenja.FindAsync(id);

            if (sesijaUcenja != null)
            {
                _context.SesijeUcenja.Remove(sesijaUcenja);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SesijaUcenjaExists(int id)
        {
            return _context.SesijeUcenja.Any(e => e.IdSesije == id);
        }
    }
}
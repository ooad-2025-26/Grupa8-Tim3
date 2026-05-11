using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Controllers
{
    public class SesijaUcenjaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SesijaUcenjaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SesijaUcenja
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SesijeUcenja.Include(s => s.Kreator).Include(s => s.Lokacija).Include(s => s.Predmet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SesijaUcenja/Details/5
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

        // GET: SesijaUcenja/Create
        public IActionResult Create()
        {
            ViewData["KreatorId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika");
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "IdLokacije", "IdLokacije");
            ViewData["PredmetId"] = new SelectList(_context.Predmeti, "IdPredmeta", "IdPredmeta");
            return View();
        }

        // POST: SesijaUcenja/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["KreatorId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika", sesijaUcenja.KreatorId);
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "IdLokacije", "IdLokacije", sesijaUcenja.LokacijaId);
            ViewData["PredmetId"] = new SelectList(_context.Predmeti, "IdPredmeta", "IdPredmeta", sesijaUcenja.PredmetId);
            return View(sesijaUcenja);
        }

        // GET: SesijaUcenja/Edit/5
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
            ViewData["KreatorId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika", sesijaUcenja.KreatorId);
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "IdLokacije", "IdLokacije", sesijaUcenja.LokacijaId);
            ViewData["PredmetId"] = new SelectList(_context.Predmeti, "IdPredmeta", "IdPredmeta", sesijaUcenja.PredmetId);
            return View(sesijaUcenja);
        }

        // POST: SesijaUcenja/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["KreatorId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika", sesijaUcenja.KreatorId);
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "IdLokacije", "IdLokacije", sesijaUcenja.LokacijaId);
            ViewData["PredmetId"] = new SelectList(_context.Predmeti, "IdPredmeta", "IdPredmeta", sesijaUcenja.PredmetId);
            return View(sesijaUcenja);
        }

        // GET: SesijaUcenja/Delete/5
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

        // POST: SesijaUcenja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sesijaUcenja = await _context.SesijeUcenja.FindAsync(id);
            if (sesijaUcenja != null)
            {
                _context.SesijeUcenja.Remove(sesijaUcenja);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SesijaUcenjaExists(int id)
        {
            return _context.SesijeUcenja.Any(e => e.IdSesije == id);
        }
    }
}

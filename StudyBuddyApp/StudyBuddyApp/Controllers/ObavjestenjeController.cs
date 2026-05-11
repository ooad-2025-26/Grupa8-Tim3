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
    public class ObavjestenjeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ObavjestenjeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Obavjestenje
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Obavjestenja.Include(o => o.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Obavjestenje/Details/5
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

        // GET: Obavjestenje/Create
        public IActionResult Create()
        {
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika");
            return View();
        }

        // POST: Obavjestenje/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika", obavjestenje.KorisnikId);
            return View(obavjestenje);
        }

        // GET: Obavjestenje/Edit/5
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
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika", obavjestenje.KorisnikId);
            return View(obavjestenje);
        }

        // POST: Obavjestenje/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika", obavjestenje.KorisnikId);
            return View(obavjestenje);
        }

        // GET: Obavjestenje/Delete/5
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

        // POST: Obavjestenje/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obavjestenje = await _context.Obavjestenja.FindAsync(id);
            if (obavjestenje != null)
            {
                _context.Obavjestenja.Remove(obavjestenje);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObavjestenjeExists(int id)
        {
            return _context.Obavjestenja.Any(e => e.IdObavjestenja == id);
        }
    }
}

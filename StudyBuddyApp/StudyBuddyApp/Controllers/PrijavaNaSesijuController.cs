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
    public class PrijavaNaSesijuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrijavaNaSesijuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PrijavaNaSesiju
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PrijaveNaSesije.Include(p => p.Korisnik).Include(p => p.SesijaUcenja);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PrijavaNaSesiju/Details/5
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

        // GET: PrijavaNaSesiju/Create
        public IActionResult Create()
        {
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika");
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "IdSesije");
            return View();
        }

        // POST: PrijavaNaSesiju/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika", prijavaNaSesiju.KorisnikId);
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "IdSesije", prijavaNaSesiju.SesijaId);
            return View(prijavaNaSesiju);
        }

        // GET: PrijavaNaSesiju/Edit/5
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
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika", prijavaNaSesiju.KorisnikId);
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "IdSesije", prijavaNaSesiju.SesijaId);
            return View(prijavaNaSesiju);
        }

        // POST: PrijavaNaSesiju/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika", prijavaNaSesiju.KorisnikId);
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "IdSesije", prijavaNaSesiju.SesijaId);
            return View(prijavaNaSesiju);
        }

        // GET: PrijavaNaSesiju/Delete/5
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

        // POST: PrijavaNaSesiju/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prijavaNaSesiju = await _context.PrijaveNaSesije.FindAsync(id);
            if (prijavaNaSesiju != null)
            {
                _context.PrijaveNaSesije.Remove(prijavaNaSesiju);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrijavaNaSesijuExists(int id)
        {
            return _context.PrijaveNaSesije.Any(e => e.IdPrijave == id);
        }
    }
}

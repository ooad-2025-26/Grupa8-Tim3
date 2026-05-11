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
    public class PrisustvoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrisustvoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Prisustvo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Prisustva.Include(p => p.Korisnik).Include(p => p.SesijaUcenja);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Prisustvo/Details/5
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

        // GET: Prisustvo/Create
        public IActionResult Create()
        {
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika");
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "IdSesije");
            return View();
        }

        // POST: Prisustvo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika", prisustvo.KorisnikId);
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "IdSesije", prisustvo.SesijaId);
            return View(prisustvo);
        }

        // GET: Prisustvo/Edit/5
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
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika", prisustvo.KorisnikId);
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "IdSesije", prisustvo.SesijaId);
            return View(prisustvo);
        }

        // POST: Prisustvo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "IdKorisnika", "IdKorisnika", prisustvo.KorisnikId);
            ViewData["SesijaId"] = new SelectList(_context.SesijeUcenja, "IdSesije", "IdSesije", prisustvo.SesijaId);
            return View(prisustvo);
        }

        // GET: Prisustvo/Delete/5
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

        // POST: Prisustvo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prisustvo = await _context.Prisustva.FindAsync(id);
            if (prisustvo != null)
            {
                _context.Prisustva.Remove(prisustvo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrisustvoExists(int id)
        {
            return _context.Prisustva.Any(e => e.IdPrisustva == id);
        }
    }
}

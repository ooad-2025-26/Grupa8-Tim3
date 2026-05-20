using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UlogaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UlogaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Uloge.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uloga = await _context.Uloge
                .FirstOrDefaultAsync(m => m.IdUloge == id);

            if (uloga == null)
            {
                return NotFound();
            }

            return View(uloga);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUloge,Naziv,Opis")] Uloga uloga)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uloga);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(uloga);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uloga = await _context.Uloge.FindAsync(id);

            if (uloga == null)
            {
                return NotFound();
            }

            return View(uloga);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUloge,Naziv,Opis")] Uloga uloga)
        {
            if (id != uloga.IdUloge)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uloga);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UlogaExists(uloga.IdUloge))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(uloga);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uloga = await _context.Uloge
                .FirstOrDefaultAsync(m => m.IdUloge == id);

            if (uloga == null)
            {
                return NotFound();
            }

            return View(uloga);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uloga = await _context.Uloge.FindAsync(id);

            if (uloga != null)
            {
                _context.Uloge.Remove(uloga);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UlogaExists(int id)
        {
            return _context.Uloge.Any(e => e.IdUloge == id);
        }
    }
}
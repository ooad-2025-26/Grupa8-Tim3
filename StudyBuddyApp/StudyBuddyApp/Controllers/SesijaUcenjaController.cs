using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;
using StudyBuddyApp.Services;

namespace StudyBuddyApp.Controllers
{
    [Authorize]
    public class SesijaUcenjaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;
        private readonly ObavjestenjeFactory _obavjestenjeFactory;

        public SesijaUcenjaController(
            ApplicationDbContext context,
            UserManager<Korisnik> userManager,
            ObavjestenjeFactory obavjestenjeFactory)
        {
            _context = context;
            _userManager = userManager;
            _obavjestenjeFactory = obavjestenjeFactory;
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

            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik != null && User.IsInRole("Student"))
            {
                var aktivnaPrijava = await _context.PrijaveNaSesije
                    .FirstOrDefaultAsync(p =>
                        p.SesijaId == sesijaUcenja.IdSesije &&
                        p.KorisnikId == korisnik.Id &&
                        p.StatusPrijave == StatusPrijave.Prijavljen);

                ViewBag.VecPrijavljen = aktivnaPrijava != null;
                ViewBag.IdPrijave = aktivnaPrijava?.IdPrijave;
            }
            else
            {
                ViewBag.VecPrijavljen = false;
                ViewBag.IdPrijave = null;
            }

            return View(sesijaUcenja);
        }

        [Authorize(Roles = "Administrator,Student")]
        public IActionResult Create()
        {
            PopuniPadajuceListe();
            return View();
        }

        [Authorize(Roles = "Administrator,Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSesije,Naziv,Opis,DatumVrijeme,Trajanje,LokacijaId,PredmetId,MaksimalanBrojUcesnika,BrojSlobodnihMjesta,StatusSesije")] SesijaUcenja sesijaUcenja)
        {
            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik == null)
            {
                return Challenge();
            }

            sesijaUcenja.KreatorId = korisnik.Id;

            if (sesijaUcenja.MaksimalanBrojUcesnika <= 0)
            {
                ModelState.AddModelError("MaksimalanBrojUcesnika", "Maksimalan broj učesnika mora biti veći od 0.");
            }

            if (sesijaUcenja.BrojSlobodnihMjesta < 0)
            {
                ModelState.AddModelError("BrojSlobodnihMjesta", "Broj slobodnih mjesta ne može biti negativan.");
            }

            if (sesijaUcenja.BrojSlobodnihMjesta > sesijaUcenja.MaksimalanBrojUcesnika)
            {
                ModelState.AddModelError("BrojSlobodnihMjesta", "Broj slobodnih mjesta ne može biti veći od maksimalnog broja učesnika.");
            }

            if (sesijaUcenja.Trajanje <= 0)
            {
                ModelState.AddModelError("Trajanje", "Trajanje sesije mora biti veće od 0.");
            }

            if (ModelState.IsValid)
            {
                _context.SesijeUcenja.Add(sesijaUcenja);
                await _context.SaveChangesAsync();

                TempData["Poruka"] = "Sesija je uspješno kreirana.";
                return RedirectToAction(nameof(Index));
            }

            PopuniPadajuceListe(sesijaUcenja.LokacijaId, sesijaUcenja.PredmetId);
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

            PopuniPadajuceListe(sesijaUcenja.LokacijaId, sesijaUcenja.PredmetId);
            ViewData["KreatorId"] = new SelectList(_context.Users, "Id", "Ime", sesijaUcenja.KreatorId);

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

            if (sesijaUcenja.BrojSlobodnihMjesta > sesijaUcenja.MaksimalanBrojUcesnika)
            {
                ModelState.AddModelError("BrojSlobodnihMjesta", "Broj slobodnih mjesta ne može biti veći od maksimalnog broja učesnika.");
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

            PopuniPadajuceListe(sesijaUcenja.LokacijaId, sesijaUcenja.PredmetId);
            ViewData["KreatorId"] = new SelectList(_context.Users, "Id", "Ime", sesijaUcenja.KreatorId);

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

        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {
            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik == null)
            {
                return Challenge();
            }

            var sesija = await _context.SesijeUcenja
                .FirstOrDefaultAsync(s => s.IdSesije == id);

            if (sesija == null)
            {
                return NotFound();
            }

            if (sesija.StatusSesije != StatusSesije.Aktivna)
            {
                TempData["Greska"] = "Prijava nije moguća jer sesija nije aktivna.";
                return RedirectToAction(nameof(Details), new { id });
            }

            if (sesija.BrojSlobodnihMjesta <= 0)
            {
                TempData["Greska"] = "Prijava nije moguća jer nema slobodnih mjesta.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var vecPrijavljen = await _context.PrijaveNaSesije
                .AnyAsync(p =>
                    p.SesijaId == id &&
                    p.KorisnikId == korisnik.Id &&
                    p.StatusPrijave == StatusPrijave.Prijavljen);

            if (vecPrijavljen)
            {
                TempData["Greska"] = "Već ste prijavljeni na ovu sesiju.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var prijava = new PrijavaNaSesiju
            {
                DatumPrijave = DateTime.Now,
                KorisnikId = korisnik.Id,
                SesijaId = sesija.IdSesije,
                StatusPrijave = StatusPrijave.Prijavljen
            };

            sesija.BrojSlobodnihMjesta--;

            var obavjestenje = _obavjestenjeFactory.KreirajObavjestenje(
                TipObavjestenja.Prijava,
                korisnik,
                sesija);

            _context.PrijaveNaSesije.Add(prijava);
            _context.Obavjestenja.Add(obavjestenje);
            await _context.SaveChangesAsync();

            TempData["Poruka"] = "Uspješno ste se prijavili na sesiju.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelJoin(int id)
        {
            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik == null)
            {
                return Challenge();
            }

            var prijava = await _context.PrijaveNaSesije
                .Include(p => p.SesijaUcenja)
                .FirstOrDefaultAsync(p =>
                    p.IdPrijave == id &&
                    p.KorisnikId == korisnik.Id &&
                    p.StatusPrijave == StatusPrijave.Prijavljen);

            if (prijava == null)
            {
                TempData["Greska"] = "Aktivna prijava nije pronađena.";
                return RedirectToAction(nameof(Index));
            }

            if (prijava.SesijaUcenja == null)
            {
                return NotFound();
            }

            prijava.StatusPrijave = StatusPrijave.Otkazan;
            prijava.SesijaUcenja.BrojSlobodnihMjesta++;

            var obavjestenje = _obavjestenjeFactory.KreirajObavjestenje(
                TipObavjestenja.Otkazivanje,
                korisnik,
                prijava.SesijaUcenja);

            _context.Obavjestenja.Add(obavjestenje);
            await _context.SaveChangesAsync();

            TempData["Poruka"] = "Uspješno ste otkazali prijavu.";
            return RedirectToAction(nameof(Details), new { id = prijava.SesijaId });
        }

        private void PopuniPadajuceListe(int? lokacijaId = null, int? predmetId = null)
        {
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "IdLokacije", "Naziv", lokacijaId);
            ViewData["PredmetId"] = new SelectList(_context.Predmeti, "IdPredmeta", "Naziv", predmetId);
        }

        private bool SesijaUcenjaExists(int id)
        {
            return _context.SesijeUcenja.Any(e => e.IdSesije == id);
        }
    }
}
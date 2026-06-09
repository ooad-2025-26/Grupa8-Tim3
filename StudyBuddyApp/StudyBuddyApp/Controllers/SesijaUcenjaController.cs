using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;
using StudyBuddyApp.Services.Observers;
using StudyBuddyApp.Services.Sessions;

namespace StudyBuddyApp.Controllers
{
    [Authorize]
    public class SesijaUcenjaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;
        private readonly SesijaFacade _sesijaFacade;
        private readonly SesijaSubject _sesijaSubject;

        public SesijaUcenjaController(
            ApplicationDbContext context,
            UserManager<Korisnik> userManager,
            SesijaFacade sesijaFacade,
            SesijaSubject sesijaSubject)
        {
            _context = context;
            _userManager = userManager;
            _sesijaFacade = sesijaFacade;
            _sesijaSubject = sesijaSubject;
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
        public async Task<IActionResult> Create([Bind("IdSesije,Naziv,Opis,DatumVrijeme,Trajanje,LokacijaId,PredmetId,MaksimalanBrojUcesnika,StatusSesije")] SesijaUcenja sesijaUcenja)
        {
            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik == null)
            {
                return Challenge();
            }

            var rezultat = await _sesijaFacade.KreirajSesijuAsync(sesijaUcenja, korisnik);

            if (rezultat.Uspjesno)
            {
                TempData["Poruka"] = rezultat.Poruka;
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, rezultat.Poruka ?? "Došlo je do greške prilikom kreiranja sesije.");

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

            if (ModelState.IsValid)
            {
                var staraSesija = await _context.SesijeUcenja
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.IdSesije == id);

                if (staraSesija == null)
                {
                    return NotFound();
                }

                var promjena = new SesijaPromjenaInfo
                {
                    SesijaId = id,
                    StariDatumVrijeme = staraSesija.DatumVrijeme,
                    NoviDatumVrijeme = DateTime.SpecifyKind(sesijaUcenja.DatumVrijeme, DateTimeKind.Utc),
                    StaraLokacijaId = staraSesija.LokacijaId,
                    NovaLokacijaId = sesijaUcenja.LokacijaId,
                    StariStatus = staraSesija.StatusSesije,
                    NoviStatus = sesijaUcenja.StatusSesije
                };

                sesijaUcenja.DatumVrijeme = DateTime.SpecifyKind(sesijaUcenja.DatumVrijeme, DateTimeKind.Utc);

                try
                {
                    _context.Update(sesijaUcenja);
                    await _context.SaveChangesAsync();

                    await _sesijaSubject.ObavijestiObservereAsync(promjena);

                    TempData["Poruka"] = "Sesija je uspješno uređena.";
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

            var rezultat = await _sesijaFacade.PrijaviKorisnikaNaSesijuAsync(id, korisnik);

            if (rezultat.Uspjesno)
            {
                TempData["Poruka"] = rezultat.Poruka;
            }
            else
            {
                TempData["Greska"] = rezultat.Poruka;
            }

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

            var (rezultat, sesijaId) = await _sesijaFacade.OtkaziPrijavuAsync(id, korisnik);

            if (rezultat.Uspjesno)
            {
                TempData["Poruka"] = rezultat.Poruka;
            }
            else
            {
                TempData["Greska"] = rezultat.Poruka;
            }

            if (sesijaId.HasValue)
            {
                return RedirectToAction(nameof(Details), new { id = sesijaId.Value });
            }

            return RedirectToAction(nameof(Index));
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
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

        public async Task<IActionResult> Index(
            string? pretraga,
            GodinaStudija? godinaStudija,
            SmjerStudija? smjerStudija,
            [FromQuery] int? predmetId,
            StatusSesije? statusSesije,
            bool samoSlobodnaMjesta = false,
            string? sortiranje = "datum")
        {
            var sesije = _context.SesijeUcenja
                .Include(s => s.Kreator)
                .Include(s => s.Lokacija)
                .Include(s => s.Predmet)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pretraga))
            {
                var pojam = pretraga.ToLower();

                sesije = sesije.Where(s =>
                    s.Naziv.ToLower().Contains(pojam) ||
                    s.Opis.ToLower().Contains(pojam) ||
                    s.Predmet!.Naziv.ToLower().Contains(pojam) ||
                    s.Lokacija!.Naziv.ToLower().Contains(pojam));
            }

            if (godinaStudija.HasValue)
            {
                sesije = sesije.Where(s => s.Predmet!.GodinaStudija == godinaStudija.Value);
            }

            if (smjerStudija.HasValue)
            {
                sesije = sesije.Where(s => s.Predmet!.SmjerStudija == smjerStudija.Value);
            }

            if (predmetId.HasValue)
            {
                sesije = sesije.Where(s => s.PredmetId == predmetId.Value);
            }

            if (statusSesije.HasValue)
            {
                sesije = sesije.Where(s => s.StatusSesije == statusSesije.Value);
            }

            if (samoSlobodnaMjesta)
            {
                sesije = sesije.Where(s => s.BrojSlobodnihMjesta > 0);
            }

            sesije = sortiranje switch
            {
                "datum_desc" => sesije.OrderByDescending(s => s.DatumVrijeme),
                "mjesta" => sesije.OrderBy(s => s.BrojSlobodnihMjesta),
                "mjesta_desc" => sesije.OrderByDescending(s => s.BrojSlobodnihMjesta),
                "naziv" => sesije.OrderBy(s => s.Naziv),
                _ => sesije.OrderBy(s => s.DatumVrijeme)
            };

            ViewBag.Pretraga = pretraga;
            ViewBag.GodinaStudija = godinaStudija?.ToString();
            ViewBag.SmjerStudija = smjerStudija?.ToString();
            ViewBag.PredmetId = predmetId;
            ViewBag.StatusSesijeOdabrano = statusSesije;
            ViewBag.SamoSlobodnaMjesta = samoSlobodnaMjesta;
            ViewBag.Sortiranje = sortiranje;

            var predmetiZaFilter = _context.Predmeti.AsQueryable();

            if (godinaStudija.HasValue)
            {
                predmetiZaFilter = predmetiZaFilter.Where(p => p.GodinaStudija == godinaStudija.Value);
            }

            if (smjerStudija.HasValue)
            {
                predmetiZaFilter = predmetiZaFilter.Where(p => p.SmjerStudija == smjerStudija.Value);
            }

            ViewBag.Predmeti = await predmetiZaFilter
                .OrderBy(p => p.Naziv)
                .ToListAsync();

            ViewBag.StatusiSesije = new SelectList(
                Enum.GetValues(typeof(StatusSesije))
                    .Cast<StatusSesije>()
                    .Select(s => new
                    {
                        Value = s.ToString(),
                        Text = s.ToString()
                    }),
                "Value",
                "Text",
                statusSesije?.ToString()
            );

            ViewBag.Sortiranja = new SelectList(
                new[]
                {
            new { Value = "datum", Text = "Datum: prvo najranije" },
            new { Value = "datum_desc", Text = "Datum: prvo najkasnije" },
            new { Value = "mjesta_desc", Text = "Slobodna mjesta: najviše prvo" },
            new { Value = "mjesta", Text = "Slobodna mjesta: najmanje prvo" },
            new { Value = "naziv", Text = "Naziv sesije" }
                },
                "Value",
                "Text",
                sortiranje
            );

            return View(await sesije.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetPredmetiByGodinaISmjer(GodinaStudija? godinaStudija, SmjerStudija? smjerStudija)
        {
            var predmeti = _context.Predmeti.AsQueryable();

            if (godinaStudija.HasValue)
            {
                predmeti = predmeti.Where(p => p.GodinaStudija == godinaStudija.Value);
            }

            if (smjerStudija.HasValue)
            {
                predmeti = predmeti.Where(p => p.SmjerStudija == smjerStudija.Value);
            }

            var rezultat = await predmeti
                .OrderBy(p => p.Naziv)
                .Select(p => new
                {
                    id = p.IdPredmeta,
                    naziv = p.Naziv
                })
                .ToListAsync();

            return Json(rezultat);
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
        public async Task<IActionResult> Create(
    [Bind("IdSesije,Naziv,Opis,DatumVrijeme,Trajanje,LokacijaId,PredmetId,MaksimalanBrojUcesnika,StatusSesije")] SesijaUcenja sesijaUcenja,
    string? NovaLokacijaNaziv,
    TipLokacije? NovaLokacijaTip,
    string? NovaLokacijaAdresa,
    string? NovaLokacijaLink)
        {
            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik == null)
            {
                return Challenge();
            }

            if (sesijaUcenja.LokacijaId == -1)
            {
                if (string.IsNullOrWhiteSpace(NovaLokacijaNaziv))
                {
                    ModelState.AddModelError(string.Empty, "Naziv nove lokacije je obavezan.");
                    PopuniPadajuceListe(sesijaUcenja.LokacijaId, sesijaUcenja.PredmetId);
                    return View(sesijaUcenja);
                }

                if (!NovaLokacijaTip.HasValue)
                {
                    ModelState.AddModelError(string.Empty, "Tip nove lokacije je obavezan.");
                    PopuniPadajuceListe(sesijaUcenja.LokacijaId, sesijaUcenja.PredmetId);
                    return View(sesijaUcenja);
                }

                if (NovaLokacijaTip.Value == TipLokacije.Fizicka && string.IsNullOrWhiteSpace(NovaLokacijaAdresa))
                {
                    ModelState.AddModelError(string.Empty, "Adresa je obavezna za fizičku lokaciju.");
                    PopuniPadajuceListe(sesijaUcenja.LokacijaId, sesijaUcenja.PredmetId);
                    return View(sesijaUcenja);
                }

                if (NovaLokacijaTip.Value == TipLokacije.Online && string.IsNullOrWhiteSpace(NovaLokacijaLink))
                {
                    NovaLokacijaLink = "https://meet.google.com/new";
                }

                var postojecaLokacija = await _context.Lokacije.FirstOrDefaultAsync(l =>
                    l.Naziv == NovaLokacijaNaziv.Trim() &&
                    l.TipLokacije == NovaLokacijaTip.Value);

                if (postojecaLokacija != null)
                {
                    sesijaUcenja.LokacijaId = postojecaLokacija.IdLokacije;
                }
                else
                {
                    var novaLokacija = new Lokacija
                    {
                        Naziv = NovaLokacijaNaziv.Trim(),
                        TipLokacije = NovaLokacijaTip.Value,
                        Adresa = NovaLokacijaTip.Value == TipLokacije.Fizicka
                            ? NovaLokacijaAdresa?.Trim() ?? string.Empty
                            : string.Empty,
                        Link = NovaLokacijaTip.Value == TipLokacije.Online
                            ? NovaLokacijaLink?.Trim() ?? "https://meet.google.com/new"
                            : string.Empty
                    };

                    _context.Lokacije.Add(novaLokacija);
                    await _context.SaveChangesAsync();

                    sesijaUcenja.LokacijaId = novaLokacija.IdLokacije;
                }
            }

            var rezultat = await _sesijaFacade.KreirajSesijuAsync(sesijaUcenja, korisnik);

            if (rezultat.Uspjesno)
            {
                TempData["Poruka"] = rezultat.Poruka;
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, rezultat.Poruka ?? "Došlo je do greške prilikom kreiranja sesije.");

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
                TempData["Greška"] = rezultat.Poruka;
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
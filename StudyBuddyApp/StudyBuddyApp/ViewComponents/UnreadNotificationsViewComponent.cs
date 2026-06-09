using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.ViewComponents
{
    public class UnreadNotificationsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public UnreadNotificationsViewComponent(
            ApplicationDbContext context,
            UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return View(0);
            }

            var korisnik = await _userManager.GetUserAsync(HttpContext.User);

            if (korisnik == null)
            {
                return View(0);
            }

            var brojNeprocitanih = await _context.Obavjestenja
                .CountAsync(o => o.KorisnikId == korisnik.Id && !o.Procitano);

            return View(brojNeprocitanih);
        }
    }
}
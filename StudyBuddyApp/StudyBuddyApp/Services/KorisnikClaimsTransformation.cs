using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using StudyBuddyApp.Models;
using System.Security.Claims;

namespace StudyBuddyApp.Services
{
    public class KorisnikClaimsTransformation : IClaimsTransformation
    {
        private readonly UserManager<Korisnik> _userManager;

        public KorisnikClaimsTransformation(UserManager<Korisnik> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                return principal;
            }

            var identity = principal.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return principal;
            }

            if (identity.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                return principal;
            }

            var korisnik = await _userManager.GetUserAsync(principal);

            if (korisnik == null)
            {
                return principal;
            }

            var role = await _userManager.GetRolesAsync(korisnik);

            foreach (var r in role)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, r));
            }

            return principal;
        }
    }
}
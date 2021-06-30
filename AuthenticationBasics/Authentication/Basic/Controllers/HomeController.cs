using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basic.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public async Task<IActionResult> Authenticate()
        {
            var webTechClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Farzin"),
                new Claim(ClaimTypes.Email,"Farzin@gmail.com")
            };
            var webtechIdentity = new ClaimsIdentity(webTechClaims, "Webtech Identity");

            var licenceClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Farzin Faghir"),
                new Claim("DrivingLicenes","A+")
            };
            var licenceIdentity = new ClaimsIdentity(licenceClaims, "Licence Identity");

            var userPrincipal = new ClaimsPrincipal(new[] { webtechIdentity, licenceIdentity });

            await HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}

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
        private readonly IAuthorizationService _authorizationService;

        public HomeController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [Authorize(Policy = "Claim.DOB")]
        public IActionResult SecretPolicy()
        {
            return View();
        }

        [AllowAnonymous]
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

        public async Task<IActionResult> DoStuff()
        {
            // we are doing stuff here
            var builder = new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("Hello").Build();
            var authResult=await _authorizationService.AuthorizeAsync(User, customPolicy);
            if(authResult.Succeeded)
            {

            }
            return View("Index");
        }
    }
}

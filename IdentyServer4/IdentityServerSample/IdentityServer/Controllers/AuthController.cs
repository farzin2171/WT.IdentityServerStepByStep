﻿using IdentityServer.ViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IIdentityServerInteractionService identityServerInteractionService )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _identityServerInteractionService = identityServerInteractionService;
        }

        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _identityServerInteractionService.GetLogoutContextAsync(logoutId);

            if(string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(logoutRequest.PostLogoutRedirectUri);

        }
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl=returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            // chechk if the model is valid
            var user = await _userManager.FindByNameAsync(vm.UserName);
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false);
            if (result.Succeeded)
            {
                return Redirect(vm.ReturnUrl);
            }
            return View();
        }

        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            // chechk if the model is valid

            var user = new IdentityUser(vm.UserName);
            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(vm.ReturnUrl);
            }
            return View();
        }
    }
}

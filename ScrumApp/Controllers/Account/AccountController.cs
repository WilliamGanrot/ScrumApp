using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScrumApp.Models;
using ScrumApp.Models.Account;

namespace ScrumApp.Controllers.Account
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    Email = userRegister.Email,
                    UserName = userRegister.UserName,
                    UserNameSlug = userRegister.UserName.ToLower().Replace(" ", "-")
                };

                IdentityResult result = await userManager.CreateAsync(appUser, userRegister.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, "Standard");
                    return RedirectToAction("index", "home");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(userRegister);
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl) => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await userManager.FindByNameAsync(userLogin.UserName);
                if (appUser != null)
                {
                    var result = await signInManager.PasswordSignInAsync(appUser, userLogin.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("index", "home");
                    }
                    ModelState.AddModelError("", "Login failed, wrong username or password");
                }
            }
            return View(userLogin);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
    }
}
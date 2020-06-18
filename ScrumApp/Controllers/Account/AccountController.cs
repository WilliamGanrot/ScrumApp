using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment webHostEnvironment;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.webHostEnvironment = webHostEnvironment;
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
                string FirstName = userRegister.FirstName.First().ToString().ToUpper() + userRegister.FirstName.Substring(1).ToLower();
                string LastName = userRegister.LastName.First().ToString().ToUpper() + userRegister.LastName.Substring(1).ToLower();
                string UserName = FirstName + LastName;


                string initials = FirstName[0].ToString() + LastName[0].ToString();

                List<string> colorList = new List<string> { "Yellow", "Blue", "Red", "Green", "Brown", "Ivory", "Teal", "Purple", "Orange", "Maroon", "Aquamarine", "Coral", "Fuchsia", "Wheat", "Lime", "Crimson", "Khaki", "Magenta", "Olden", "Plum", "Olive", "Cyan" };

                Random random = new Random();
                int index = random.Next(colorList.Count());

                int width = 200;
                int height = 200;

                Bitmap bmp = new Bitmap(width, height);

                Graphics graphics = Graphics.FromImage(bmp);
                Rectangle rect = new Rectangle(0, 0, width, height);

                Color color = Color.FromName(colorList[index]);
                Brush backgroundBrush = new SolidBrush(color);

                Color textColor = Color.FromName("White");
                Font font = new Font("Arial", 80, FontStyle.Bold, GraphicsUnit.Pixel);
                SizeF textSize = graphics.MeasureString(initials, font);
                Brush textBrush = new SolidBrush(textColor);

                graphics.FillRectangle(backgroundBrush, rect);
                graphics.DrawString(initials, font, textBrush, (int)((width - textSize.Width) / 2), (int)((height - textSize.Height) / 2));

                string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media\\Users");
                string filePath = Path.Combine(uploadsDir, userRegister.Email + "_profile.png");

                bmp.Save(filePath);


                AppUser appUser = new AppUser
                {
                    Email = userRegister.Email,
                    UserName = UserName.Replace(" ", ""),
                    UserNameSlug = UserName.Replace(" ", ""),
                    FirstName = FirstName,
                    LastName = LastName,
                    ProfilePicture = userRegister.Email + "_profile.png" //should generate uniqe one instead
                };

                IdentityResult result = await userManager.CreateAsync(appUser, userRegister.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, "Standard");
                    return RedirectToAction("index", "home");
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
                        return RedirectToAction("Index", "Projects");
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

        public async Task<string> GetProfilePictureAsync()
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            return user.ProfilePicture;
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScrumApp.Models;

namespace ScrumApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountsController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = userManager.Users;
            ViewBag.roles = roleManager.Roles;
            return View(users);
        }

        public async Task<IActionResult> UsersByRole(string role)
        {
            var users = await userManager.GetUsersInRoleAsync(role);

            ViewBag.roles = roleManager.Roles;
            return View("Index", users);
        }

        public async Task<IActionResult> Remove(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            await userManager.DeleteAsync(user);

            return RedirectToAction("Index");

        }


    }
}
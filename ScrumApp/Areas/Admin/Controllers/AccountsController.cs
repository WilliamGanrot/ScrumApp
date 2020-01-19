using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumApp.Data;
using ScrumApp.Models;

namespace ScrumApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountsController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ScrumApplicationContext context;

        public AccountsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ScrumApplicationContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
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
            //context.Projects.RemoveRange(user);

            if (user == null)
                return NotFound();
            
            //ask on stackoverflow??
            foreach(var p in user.Projects)
            {
                context.Projects.Remove(p);
            }


            await context.SaveChangesAsync();
            await userManager.DeleteAsync(user);

            return RedirectToAction("Index");

        }


    }
}
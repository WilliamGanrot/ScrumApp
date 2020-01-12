using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumApp.Data;
using ScrumApp.Models;

namespace ScrumApp.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ScrumAppContext context;
        private readonly UserManager<AppUser> userManager;

        public ProjectsController(ScrumAppContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var projects = context.Projects.Include(x => x.Author);
            return View(projects);
        }



        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserProject userProject)
        {

            if (ModelState.IsValid)
            {
                AppUser appUser = await userManager.GetUserAsync(HttpContext.User);
                userProject.Author = appUser;

                string slug = userProject.Name.ToLower().Replace(" ", "-");
                userProject.Slug = slug;

                context.Projects.Add(userProject);
                await context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(userProject);
        }

        public async Task<IActionResult> Remove(int id)
        {

            UserProject userProject = await context.Projects.FindAsync(id);


            context.Projects.Remove(userProject);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
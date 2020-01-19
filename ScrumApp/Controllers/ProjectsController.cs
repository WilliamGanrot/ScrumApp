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
        private readonly ScrumApplicationContext context;
        private readonly UserManager<AppUser> userManager;

        public ProjectsController(ScrumApplicationContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            //var projects = context.Projects;
            var projects = context.Projects.Include(x => x.Author);
            return View(projects);
        }



        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            
            if (ModelState.IsValid)
            {
                AppUser appUser = await userManager.GetUserAsync(HttpContext.User);
                
                project.Author = appUser;
                project.AuthorId = appUser.Id;
                
                context.Projects.Add(project);
                await context.SaveChangesAsync();

                //appUser.Projects.Add(userProject);
                appUser.Projects.Add(project);
                await userManager.UpdateAsync(appUser);

                return RedirectToAction("Index");
            }
            return View(project);
        }

        public async Task<IActionResult> Remove(int id)
        {

            Project userProject = await context.Projects.FindAsync(id);


            context.Projects.Remove(userProject);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> tempAddProject()
        {
            AppUser appUser = await userManager.GetUserAsync(HttpContext.User);


            Project project = new Project
            {
                Name = "temp-project",
                Author = appUser
            };

            context.Projects.Add(project);
            await context.SaveChangesAsync();


            //appUser.Projects.Add(project);

            await userManager.UpdateAsync(appUser);

            return Ok();
        }
    }
}
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
                var projects = context.Projects.Include(x => x.Author);
                return View(projects);

        }

        public IActionResult Details(int id)
        {
            var project = context.Projects
                .Where(x => x.ProjectId == id)
                .Include(x => x.UserProjects)
                .ThenInclude(x => x.AppUser)
                .First();

            foreach (var p in project.UserProjects)
            {
                System.Diagnostics.Debug.WriteLine(p.AppUser.UserName);
            }

            return View(project);
        }

        public async Task<IActionResult> Remove(int id)
        {
            //Not done
            Project userProject = await context.Projects.FindAsync(id);


            context.Projects.Remove(userProject);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
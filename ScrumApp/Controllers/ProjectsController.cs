using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ScrumApp.Data;
using ScrumApp.Models;
using ScrumApp.Models.Account;

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
        public async Task<IActionResult> Index()
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            var projects = context.UserProjects
                .Where(x => x.AppUser.Id == user.Id)
                .Include(x => x.Project.Author)
                .Select(x => x.Project)
                .ToList();

            foreach(var project in projects)
            {
                
            }
            return View(projects);
        }


        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProject createProject)
        {

            if (ModelState.IsValid)
            {
                AppUser user = await userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    //should return some kind of error
                    System.Diagnostics.Debug.WriteLine("User Doesn't exist");
                    return RedirectToAction("Index");
                }

                string slug = createProject.ProjectName.ToLower().Replace(" ", "-");

                Project project = new Project
                {
                    ProjectName = createProject.ProjectName,
                    Slug = slug,
                    Author = user 
                };

                project.UserProjects = new List<UserProject>
                {
                    new UserProject
                    {
                        AppUser = user,
                        Project = project
                    }
                };

                await context.Projects.AddAsync(project);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var project = context.Projects
                .Where(x => x.ProjectId == id)
                .Include(x => x.UserProjects)
                .ThenInclude(x => x.AppUser)
                .First();

            return View(project);
        }

        public async Task<IActionResult> Remove(int id)
        {

            Project userProject = await context.Projects.FindAsync(id);


            context.Projects.Remove(userProject);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public IActionResult Join()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Join(JoinProject joinProject)
        {
            Project project = await context.Projects.FindAsync(joinProject.ProjectId);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            UserProject userProject = new UserProject
            {
                AppUser = user,
                Project = project
            };
           
            project.UserProjects = new List<UserProject> { userProject };

            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
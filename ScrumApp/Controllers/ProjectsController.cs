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
using ScrumApp.Services;
using ScrumApp.Services.ProjectS;

namespace ScrumApp.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IProjectService ProjectService;

        public ProjectsController(UserManager<AppUser> userManager, IProjectService ProjectService)
        {
            this.userManager = userManager;
            this.ProjectService = ProjectService;
        }
        public async Task<IActionResult> Index()
        {

            if (!User.Identity.IsAuthenticated)
                return View("~/Views/Home/Index.cshtml");

            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            List<Project> projects = ProjectService.GetUserProjects(user);

            return View(projects);

        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProject createProject)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return RedirectToAction("Index");

            bool successful = await ProjectService.Create(createProject, user);
            System.Diagnostics.Debug.WriteLine(successful.ToString());
            if (!successful)
                return BadRequest("Could not create Project");
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Project project = ProjectService.Details(id);
            return View(project);
        }

        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            bool successful = await ProjectService.Remove(id);
            if (!successful)
                return BadRequest("Could not remove Project");
            return RedirectToAction("Index");
        }

    }
}
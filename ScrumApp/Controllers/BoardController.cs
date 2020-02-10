using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScrumApp.Data;
using ScrumApp.Models;

namespace ScrumApp.Controllers
{
    public class BoardController : Controller
    {
        private readonly ScrumApplicationContext context;
        private readonly UserManager<AppUser> userManager;

        public BoardController(ScrumApplicationContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string userSlug, string projectSlug)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            //check if the user exists
            AppUser projectOwner = context.Users
                .Where(x => x.UserName.ToLower().Replace(" ", "-") == userSlug)
                .FirstOrDefault();
            
            //if (projectOwner == null)
            //    return "Could not find user";

            //check if the projects exists
            var project = context.Projects
                .Where(x => x.ProjectName.ToLower().Replace(" ", "-") == projectSlug)
                .Where(x => x.Author == projectOwner)
                .FirstOrDefault();
            
            if (project == null)
                return NotFound();

            //check if logged in user is a member of the project
            //GIVES ERROR
            var result = context.UserProjects
                .Where(x => x.AppUser == user)
                .Where(x => x.ProjectId == project.ProjectId)
                .FirstOrDefault();

            if (result == null)
                return NotFound();


            return View(context.Boards);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string userSlug, string projectSlug)
        {
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine(userSlug + " " + projectSlug);

            }

            return View();
        }


        public string Specific(string boardSlug)
        {
            return boardSlug;
        }
    }
}
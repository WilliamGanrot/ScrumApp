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
    public class InvitationsController : Controller
    {
        private readonly ScrumApplicationContext context;
        private readonly UserManager<AppUser> userManager;

        public InvitationsController(ScrumApplicationContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult Index(string userSlug, string projectSlug)
        {
            //System.Diagnostics.Debug.WriteLine("u: " + userSlug + " p: " + projectSlug);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Invite(string userSlug, string projectSlug, ProjectInvitation projectInvitation)
        {
            //the project id with the author {userSlug} and the project {projectSlug}
            AppUser projectOwner = context.Users
                .Where(x => x.UserName.ToLower().Replace(" ", "-") == userSlug)
                .FirstOrDefault();

            var project = context.Projects
                .Where(x => x.ProjectName.ToLower().Replace(" ", "-") == projectSlug)
                .Where(x => x.Author == projectOwner)
                .FirstOrDefault();

            var projectId = project.ProjectId;
            System.Diagnostics.Debug.WriteLine(projectId);

            return RedirectToAction("Index");
        }
    }
}
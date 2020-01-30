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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Invite(ProjectInvitation projectInvitation)
        {
            System.Diagnostics.Debug.WriteLine(projectInvitation.token);
            return Ok();
        }
    }
}
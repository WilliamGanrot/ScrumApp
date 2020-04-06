using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using ScrumApp.Data;
using ScrumApp.Models;
using Microsoft.AspNetCore.Authorization;
using ScrumApp.Services.Invitation_;

namespace ScrumApp.Controllers
{
    [Authorize]
    public class InvitationsController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IInvitationService InvitationService;

        public InvitationsController(UserManager<AppUser> userManager, IInvitationService InvitationService)
        {
            this.userManager = userManager;
            this.InvitationService = InvitationService;
        }

        public IActionResult Index(string userSlug, string projectSlug)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Invite(string userSlug, string projectSlug, ProjectInvitation projectInvitation)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");
    
            AppUser projectOwner = InvitationService.GetProjectOwner(userSlug);

            Project project = InvitationService.GetProject(projectOwner, projectSlug);
            if (project == null)
                return NotFound();

            AppUser invitedUser = await userManager.FindByEmailAsync(projectInvitation.Email);

            //AppUser invitedUser = await userManager.FindByIdAsync(projectInvitation.Email);
            if (invitedUser == null)
            {
                TempData["Error"] = "The user doesn't exist";
                return RedirectToAction("Index");
            }

            if (InvitationService.UserInProject(project, invitedUser))
            {
                TempData["Error"] = "The user is allready a part of the project";
                return RedirectToAction("Index");
            }

            if (InvitationService.UserIsInvited(project, invitedUser))
            {
                TempData["Error"] = "The user has allready been invited";
                return RedirectToAction("Index");
            }

            string token = await userManager.GenerateUserTokenAsync(invitedUser, "Default", "ProjectInvitation");
            string confirmationLink = "https://localhost:44388" + Url.Action("ConfirmInvitation", "Invitations", new { token = token });

            bool successful = await InvitationService.CreateInvitation(projectInvitation, project, invitedUser, token);
            if (!successful)
                return BadRequest("Could not create invitation");

            InvitationService.SendInvitation(invitedUser, confirmationLink);

            TempData["Success"] = "An invatation has been sent to " + invitedUser.Email;

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> ConfirmInvitation(string token)
        {
            System.Diagnostics.Debug.WriteLine("confirming invitation");
            ProjectInvitation invitation = await InvitationService.GetInvitation(token);
            if (invitation == null)
            {
                TempData["Error"] = "This invitation is no longer valid, or you are allready a member of the project";
                return Redirect("/");
            }

            AppUser invitedUser = await userManager.FindByEmailAsync(invitation.Email);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if(user == invitedUser)
            {
                bool successful = await InvitationService.AddUserToProject(invitedUser, invitation);
                return Redirect("/");
            }
            else
            {
                return Unauthorized();
            }
        }


    }
}
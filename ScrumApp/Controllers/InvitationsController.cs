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

namespace ScrumApp.Controllers
{
    [Authorize]
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
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Invite(string userSlug, string projectSlug, ProjectInvitation projectInvitation)
        {

            if (ModelState.IsValid)
            {
                AppUser projectOwner = context.Users
                    .Where(x => x.UserName.ToLower().Replace(" ", "-") == userSlug)
                    .FirstOrDefault();

                var project = context.Projects
                    .Where(x => x.ProjectName.ToLower().Replace(" ", "-") == projectSlug)
                    .Where(x => x.Author == projectOwner)
                    .FirstOrDefault();

                if (project == null)
                {
                    System.Diagnostics.Debug.WriteLine("the project wasn't found");
                    return NotFound();
                }
                var projectId = project.ProjectId;

                AppUser invitedUser = await userManager.FindByIdAsync(projectInvitation.UserId);
                if (invitedUser == null)
                {
                    TempData["Error"] = "The user doesn't exist";
                    return RedirectToAction("Index");
                }

                var allreadyMemberresult = context.UserProjects
                    .Where(x => x.UserId == invitedUser.Id)
                    .Where(x => x.ProjectId == projectId)
                    .FirstOrDefault();

                if (allreadyMemberresult != null)
                {
                    TempData["Error"] = "The user is allready a part of the project";
                    return RedirectToAction("Index");
                }


                var result = context.ProjectInvitations
                    .Where(x => x.UserId == invitedUser.Id)
                    .Where(x => x.ProjectId == projectId)
                    .FirstOrDefault();

                if (result != null)
                {
                    TempData["Error"] = "The user has allready been invited";
                    return RedirectToAction("Index");
                }


                var token = await userManager.GenerateUserTokenAsync(invitedUser, "Default", "ProjectInvitation");

                projectInvitation.UserId = invitedUser.Id;
                projectInvitation.ProjectId = projectId;
                projectInvitation.token = token;

                var confirmationLink = "https://localhost:44388" + Url.Action("ConfirmInvitation", "Invitations", new { token = projectInvitation.token });

                System.Diagnostics.Debug.WriteLine(confirmationLink);

                context.ProjectInvitations.Add(projectInvitation);
                await context.SaveChangesAsync();

                /*
                SEND EMAIL
                */

                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("Easy Scrum", "easyscrumhelper@gmail.com"));

                message.To.Add(new MailboxAddress(invitedUser.UserName, invitedUser.Email));

                message.Subject = "You have been invited to a project";
                message.Body = new TextPart("plain")
                {
                    Text = "this is the invitation project, click here to accept: " + confirmationLink
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {

                    client.Connect("smtp.gmail.com", 465, true);

                    client.Authenticate("easyscrumhelper@gmail.com", "Admin_123");

                    client.Send(message);
                    client.Disconnect(true);
                }
                TempData["Success"] = "An invatation has been sent to " + invitedUser.Email;
                return RedirectToAction("Index");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Modelstate not vaild");

                var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                foreach (var error in modelStateErrors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }

                return RedirectToAction("Index");
            }

        }


        public async Task<IActionResult> ConfirmInvitation(string token)
        {

            //add user as a member to the project
            ProjectInvitation invitation = context.ProjectInvitations.Find(token);
            if (invitation == null)
            {
                TempData["Error"] = "This invitation is no longer valid, or you are allready a member of the project";
                return Redirect("/");
            }

            AppUser user = await userManager.FindByIdAsync(invitation.UserId);
            Project project = await context.Projects.FindAsync(invitation.ProjectId);
            
            if(await userManager.GetUserAsync(HttpContext.User) == user)
            {
                System.Diagnostics.Debug.WriteLine("allowed user");
                UserProject userProject = new UserProject
                {
                    AppUser = user,
                    Project = project
                };

                project.UserProjects = new List<UserProject> { userProject };

                //remove invitation from the context
                context.ProjectInvitations.Remove(invitation);
                context.SaveChanges();

                return Redirect("/");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("not allowed user");
                return Unauthorized();
            }
        }


    }
}
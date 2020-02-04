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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Invite(string userSlug, string projectSlug, ProjectInvitation projectInvitation)
        {

            if (ModelState.IsValid)
            {

                /*
                    TO DO
                    1. Make sure the email gets sent, not only generate the url 

                */
                //the project id with the author {userSlug} and the project {projectSlug}
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


                //the user being invited
                AppUser invitedUser = await userManager.FindByIdAsync(projectInvitation.UserId);
                if (invitedUser == null)
                {
                    System.Diagnostics.Debug.WriteLine("the user wasn't found");
                    return RedirectToAction("Index");
                }



                // Check if the user allready has been invited
                var result = context.ProjectInvitations
                    .Where(x => x.UserId == invitedUser.Id)
                    .Where(x => x.ProjectId == projectId)
                    .FirstOrDefault();

                if (result != null)
                {
                    //The user has allready been invited
                    //return to view with an inforamtive message
                    System.Diagnostics.Debug.WriteLine("user has allready been invited");
                    return RedirectToAction("Index");
                }

                //the token
                var token = await userManager.GenerateUserTokenAsync(invitedUser, "Default", "ProjectInvitation");

                projectInvitation.UserId = invitedUser.Id;
                projectInvitation.ProjectId = projectId;
                projectInvitation.token = token;

                var confirmationLink = "https://localhost:44388" + Url.Action("ConfirmInvitation", "Invitations", new { token = projectInvitation.token });

                System.Diagnostics.Debug.WriteLine(confirmationLink);

                context.ProjectInvitations.Add(projectInvitation);
                await context.SaveChangesAsync();

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

            /*
                TO DO
                1. Split up th function in to a get and a post method
                2. make sure that the user is authenticated before the user gets added 
                3. make sure user is a valid user
                4. make sure user isn't allready invited or is a member of the project

            */
            //add user as a member to the project
            ProjectInvitation invitation = context.ProjectInvitations.Find(token);

            Project project = await context.Projects.FindAsync(invitation.ProjectId);
            AppUser user = await userManager.FindByIdAsync(invitation.UserId);

            UserProject userProject = new UserProject
            {
                AppUser = user,
                Project = project
            };

            project.UserProjects = new List<UserProject> { userProject };

            //remove invitation from the context
            context.ProjectInvitations.Remove(invitation);
            
            context.SaveChanges();

            return RedirectToAction("Projects", "Index");
        }


    }
}
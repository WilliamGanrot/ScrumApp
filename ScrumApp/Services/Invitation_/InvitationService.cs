using Microsoft.AspNetCore.Mvc;
using MimeKit;
using ScrumApp.Data;
using ScrumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Services.Invitation_
{
    public class InvitationService : IInvitationService
    {
        private readonly ScrumApplicationContext context;
        public InvitationService(ScrumApplicationContext context)
        {
            this.context = context;
        }

        public async Task<ProjectInvitation> GetInvitation(string token)
        {
            ProjectInvitation invitation = await context.ProjectInvitations.FindAsync(token);
            return invitation;
        }

        public async Task<bool> CreateInvitation(ProjectInvitation projectInvitation, Project project, AppUser user, string token)
        {
            projectInvitation.UserId = user.Id;
            projectInvitation.ProjectId = project.ProjectId;
            projectInvitation.token = token;

            context.ProjectInvitations.Add(projectInvitation);
            int saveResult = await context.SaveChangesAsync();

            return saveResult == 1;
        }

        public Project GetProject(AppUser projectOwner, string projectSlug)
        {
            var project = context.Projects
                .Where(x => x.ProjectName.ToLower().Replace(" ", "-") == projectSlug)
                .Where(x => x.Author == projectOwner)
                .FirstOrDefault();

            return project;
        }

        public AppUser GetProjectOwner(string userSlug)
        {
            AppUser projectOwner = context.Users
                .Where(x => x.UserName.ToLower().Replace(" ", "-") == userSlug)
                .FirstOrDefault();

            return projectOwner;
        }

        public void SendInvitation(AppUser invitedUser, string confirmationLink)
        {
            MimeMessage message = new MimeMessage();

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
        }

        public bool UserInProject(Project project, AppUser user)
        {
            UserProject userProject = context.UserProjects
                .Where(x => x.UserId == user.Id)
                .Where(x => x.ProjectId == project.ProjectId)
                .FirstOrDefault();

            if (userProject != null)
                return true;
            return false;
        }

        public bool UserIsInvited(Project project, AppUser user)
        {
            ProjectInvitation invitation = context.ProjectInvitations
                .Where(x => x.UserId == user.Id)
                .Where(x => x.ProjectId == project.ProjectId)
                .FirstOrDefault();

            if (invitation != null)
                return true;
            return false;
        }

        public async Task<bool> AddUserToProject(AppUser invitedUser, ProjectInvitation invitation)
        {
            Project project = await context.Projects.FindAsync(invitation.ProjectId);
            UserProject userProject = new UserProject
            {
                AppUser = invitedUser,
                Project = project
            };

            project.UserProjects = new List<UserProject> { userProject };

            context.ProjectInvitations.Remove(invitation);
            int saveResult = await context.SaveChangesAsync();

            return saveResult == 1;
        }
    }
}

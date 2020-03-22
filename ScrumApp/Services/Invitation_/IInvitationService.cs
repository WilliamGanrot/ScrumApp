using ScrumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Services.Invitation_
{
    public interface IInvitationService
    {
        AppUser GetProjectOwner(string userSlug);
        Project GetProject(AppUser projectOwner, string projectSlug);
        bool UserInProject(Project project, AppUser user);
        bool UserIsInvited(Project project, AppUser user);
        Task<bool> CreateInvitation(ProjectInvitation projectInvitation, Project project, AppUser user, string token);
        void SendInvitation(AppUser invitedUser, string confirmationLink);
        Task<ProjectInvitation> GetInvitation(string token);
        Task<bool> AddUserToProject(AppUser invitedUser, ProjectInvitation invitation);


    }
}

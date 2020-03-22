using ScrumApp.Models;
using ScrumApp.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Services.ProjectS
{
    public interface IProjectService
    {
        List<Project> GetUserProjects(AppUser user);
        Task<bool> Create(CreateProject createProject, AppUser user);
        Project Details(int id);
        Task<bool> Remove(int id);
    }
}

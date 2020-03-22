using Microsoft.EntityFrameworkCore;
using ScrumApp.Data;
using ScrumApp.Models;
using ScrumApp.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Services.ProjectS
{
    public class ProjectService : IProjectService
    {
        private readonly ScrumApplicationContext context;
        

        public ProjectService(ScrumApplicationContext context)
        {
            this.context = context;
        }

        public async Task<bool> Create(CreateProject createProject, AppUser user)
        {
            string slug = createProject.ProjectName.ToLower().Replace(" ", "-");

            Project project = new Project
            {
                ProjectName = createProject.ProjectName,
                Slug = slug,
                Author = user
            };

            project.UserProjects = new List<UserProject>
            {
                 new UserProject
                 {
                    AppUser = user,
                    Project = project
                 }
            };

            await context.Projects.AddAsync(project);
            int saveResult = await context.SaveChangesAsync();

            System.Diagnostics.Debug.WriteLine(saveResult.ToString());
            return saveResult == 2;
        }

        public Project Details(int id)
        {
            var project = context.Projects
                .Where(x => x.ProjectId == id)
                .Include(x => x.UserProjects)
                .ThenInclude(x => x.AppUser)
                .First();

            return project;
        }

        public List<Project> GetUserProjects(AppUser user)
        {
            List<Project> projects = context.UserProjects
                .Where(x => x.AppUser.Id == user.Id)
                .Include(x => x.Project.Author)
                .Select(x => x.Project)
                .ToList();

            return projects;
        }

        public async Task<bool> Remove(int id)
        {
            Project userProject = await context.Projects.FindAsync(id);

            context.Projects.Remove(userProject);
            int saveResult = await context.SaveChangesAsync();

            return saveResult == 1;
        }
    }
}

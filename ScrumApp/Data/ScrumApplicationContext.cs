using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScrumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Data
{
    public class ScrumApplicationContext : IdentityDbContext<AppUser>
    {
        public ScrumApplicationContext(DbContextOptions<ScrumApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProject>()
                .HasKey(bc => new { bc.ProjectId, bc.UserId });
            modelBuilder.Entity<UserProject>()
                .HasOne(bc => bc.AppUser)
                .WithMany(b => b.UserProjects)
                .HasForeignKey(bc => bc.UserId);
            modelBuilder.Entity<UserProject>()
                .HasOne(bc => bc.Project)
                .WithMany(c => c.UserProjects)
                .HasForeignKey(bc => bc.ProjectId);
            
            modelBuilder.Entity<UserStory>()
                .HasKey(bc => new { bc.StoryId, bc.UserId });
            modelBuilder.Entity<UserStory>()
                .HasOne(bc => bc.AppUser)
                .WithMany(b => b.UserStories)
                .HasForeignKey(bc => bc.UserId);
            modelBuilder.Entity<UserStory>()
                .HasOne(bc => bc.Story)
                .WithMany(c => c.UserStories)
                .HasForeignKey(bc => bc.StoryId);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<UserStory> UserStories { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<ProjectInvitation> ProjectInvitations { get; set; }
        public DbSet<BoardColumn> BoardColumns { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

    }

}



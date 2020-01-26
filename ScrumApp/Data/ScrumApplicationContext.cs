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

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<Board> Boards { get; set; }
        //public DbSet<AppUser> AppUsers { get; set; }

    }

}



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
        public DbSet<Project> Projects { get; set; }

    }

}

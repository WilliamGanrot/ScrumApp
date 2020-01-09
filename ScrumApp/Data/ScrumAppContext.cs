using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScrumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Data
{
    public class ScrumAppContext : IdentityDbContext<AppUser>
    {
        public ScrumAppContext(DbContextOptions<ScrumAppContext> options)
            : base(options)
        {
        }

        public DbSet<UserProject> Projects { get; set; }
    }
}

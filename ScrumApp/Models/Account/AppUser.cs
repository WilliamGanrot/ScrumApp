using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class AppUser : IdentityUser
    {

        public string UserNameSlug { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }
    }
}

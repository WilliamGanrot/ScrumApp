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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }
        public ICollection<UserStory> UserStories { get; set; }
    }
}

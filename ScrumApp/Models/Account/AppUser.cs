using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class AppUser : IdentityUser
    {

        //public UserProject Projects { get; set; }
        //public int xxxx { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}

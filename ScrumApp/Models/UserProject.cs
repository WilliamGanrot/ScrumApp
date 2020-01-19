using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class UserProject
    {
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}

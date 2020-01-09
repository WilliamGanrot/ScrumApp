using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class UserProject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public static implicit operator UserProject(AppUser v)
        {
            throw new NotImplementedException();
        }

        //public AppUser Author { get; set; }
    }
}

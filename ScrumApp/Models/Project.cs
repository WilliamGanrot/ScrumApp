using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class Project
    {
        [Column("Id")]
        public int ProjectId { get; set; }

        [Column("Name")]
        public string ProjectName { get; set; }

        public string Slug { get; set; }


        public string AuthorId { get; set; }

        public virtual AppUser Author { get; set; }


        public ICollection<UserProject> UserProjects { get; set; }


        public ICollection<Board> Boards { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class ProjectInvitation
    {
        [Key]
        public string token { get; set; }
        public string UserId { get; set; }
        public int ProjectId { get; set; }
    }
}

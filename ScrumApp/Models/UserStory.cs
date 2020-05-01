    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class UserStory
    {
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

        public int StoryId { get; set; }
        public Story Story { get; set; }

        public DateTime Date { get; set; }
    }
}

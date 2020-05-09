using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class CreateStory
    {
        public int BoardColumnId { get; set; }
        public string StoryTitle { get; set; }
        public string StoryDescription { get; set; }
    }
}

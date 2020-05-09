using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class Story
    {
        public int StoryId { get; set; }
        public string StoryTitle { get; set; }
        public string StorySlug { get; set; }
        public string StoryDescription { get; set; }
        public int StorySorting { get; set; }

        public int BoardColumnId { get; set; }
        public BoardColumn BoardColumn { get; set; }

        public ICollection<UserStory> UserStories { get; set; }
        
    }
}

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
        public int StorySorting { get; set; }

        public int BoardColumnId { get; set; }
        public BoardColumn BoardColumn { get; set; }

        public ICollection<UserStory> UserStories { get; set; }

        
        
        public bool IsUserAssignerToStory(AppUser user)
        {
            System.Diagnostics.Debug.WriteLine("__________");
            var x = this.UserStories.Where(x => x.AppUser == user).FirstOrDefault();
            if (x != null)
            {
                System.Diagnostics.Debug.WriteLine("true");
                return true;
            }
            System.Diagnostics.Debug.WriteLine("false");
            return false;
            
        }
        
    }
}

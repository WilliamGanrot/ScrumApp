using ScrumApp.Data;
using ScrumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Services
{
    public class StoryService : IStoryService
    {
        private readonly ScrumApplicationContext context;
        public StoryService(ScrumApplicationContext context)
        {
            this.context = context;
        }

        public async Task<bool> Create(CreateStory createStory)
        {
            Story story = new Story
            {
                StoryTitle = createStory.StoryTitle,
                StorySlug = createStory.StoryTitle.ToLower().Replace(" ", "-"),
                BoardColumn = context.BoardColumns.Find(createStory.BoardColumnId),
                StorySorting = 100
            };

            await context.Stories.AddAsync(story);
            
            int saveResult = context.SaveChanges();
            return saveResult == 1;
        }

        public async Task<bool> Delete(int id)
        {
            Story story = await context.Stories.FindAsync(id);

            context.Stories.Remove(story);
            int saveResult = await context.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<bool> Reorder(int BoardColumnId, int[] vals)
        {
            BoardColumn boardColumn = await context.BoardColumns.FindAsync(BoardColumnId);
            int order = 1;

            foreach (var storyId in vals)
            {
                Story story = await context.Stories.FindAsync(storyId);
                story.BoardColumn = boardColumn;
                story.StorySorting = order;
                order += 1;

                context.Stories.Update(story);
            }
            int saveResult = await context.SaveChangesAsync();
            return saveResult == 1;
        }
        public async Task<bool> AssignToStory(int id, AppUser user)
        {
            Story story = await context.Stories.FindAsync(id);

            System.Diagnostics.Debug.WriteLine("Assigning " + user.UserName + " to " + story.StoryTitle);
            /*
            UserStory userStory = new UserStory
            {
                AppUser = user,
                Story = story
            };

            story.UserStories = new List<UserStory> { userStory };
            
            */
            story.UserStories = new List<UserStory>
{
              new UserStory {
                Story = story,
                AppUser = user
              }
            };

            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine(story.UserStories.Count.ToString());
            System.Diagnostics.Debug.WriteLine("");

            int saveResult = await context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> DissociateToStory(int id, AppUser user)
        {
            Story story = await context.Stories.FindAsync(id);

            UserStory userStory = context.UserStories
                .Where(x => x.AppUser == user)
                .Where(x => x.Story == story)
                .FirstOrDefault();

            context.UserStories.Remove(userStory);

            int saveResult = await context.SaveChangesAsync();
            return saveResult == 1;
        }
    }
    
}

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
    }
}

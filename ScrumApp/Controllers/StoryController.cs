using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScrumApp.Data;
using ScrumApp.Models;

namespace ScrumApp.Controllers
{
    public class StoryController : Controller
    {
        private readonly ScrumApplicationContext context;
        private readonly UserManager<AppUser> userManager;

        public StoryController(ScrumApplicationContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateStory createStory, string userSlug, string projectSlug, string boardSlug)
        {

            if (ModelState.IsValid)
            {
                //This item should be added to database
                Story story = new Story
                {
                    StoryTitle = createStory.StoryTitle,
                    StorySlug = createStory.StoryTitle.ToLower().Replace(" ", "-"),
                    BoardColumn = context.BoardColumns.Find(createStory.BoardColumnId),
                    StorySorting = 100
                };

                await context.Stories.AddAsync(story);
                context.SaveChanges();

                

            }

            return RedirectToAction("Index", "Board");
        }

        [HttpPost]
        public async Task<IActionResult> reorder(int id, int[] vals)
        {
            System.Diagnostics.Debug.WriteLine("BoardID: " + id);

            BoardColumn boardColumn = await context.BoardColumns.FindAsync(id);

            int order = 1;
            foreach (var storyId in vals)
            {
                Story story = await context.Stories.FindAsync(storyId);
                story.BoardColumn = boardColumn;
                story.StorySorting = order;
                order += 1;

                context.Stories.Update(story);
                await context.SaveChangesAsync();
            }
                
            //System.Diagnostics.Debug.WriteLine("StoryId: " + storyId);

            return Ok();
        }
    }
}
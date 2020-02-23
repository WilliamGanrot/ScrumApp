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
                    BoardColumn = context.BoardColumns.Find(createStory.BoardColumnId)
                };

                await context.Stories.AddAsync(story);
                context.SaveChanges();

                

            }

            return RedirectToAction("Index", "Board");
        }
    }
}
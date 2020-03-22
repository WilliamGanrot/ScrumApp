using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScrumApp.Data;
using ScrumApp.Models;
using ScrumApp.Services;

namespace ScrumApp.Controllers
{
    public class StoryController : Controller
    {
        private readonly IStoryService StoryService;
        private readonly UserManager<AppUser> userManager;

        public StoryController(UserManager<AppUser> userManager, IStoryService StoryService)
        {
            this.userManager = userManager;
            this.StoryService = StoryService;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateStory createStory)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Board");

            bool successful = await StoryService.Create(createStory);
            if (!successful)
                return BadRequest("Could not create Story");

            return RedirectToAction("Index", "Board");
        }

        [HttpPost]
        public async Task<IActionResult> reorder(int id, int[] vals)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Board");

            bool successful = await StoryService.Reorder(id, vals);
            if(!successful)
                return BadRequest("Could not reorder stories");
            return Ok();
        }
    }
}
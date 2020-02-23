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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Story story, string userSlug, string projectSlug, string boardSlug)
        {

            if (ModelState.IsValid)
            {

            }

            if(story == null)
            {
                System.Diagnostics.Debug.WriteLine("story is null");
            }

            System.Diagnostics.Debug.WriteLine("Id: " + story.BoardColumnId);
            System.Diagnostics.Debug.WriteLine(userSlug);
            System.Diagnostics.Debug.WriteLine(projectSlug);
            System.Diagnostics.Debug.WriteLine(boardSlug);
            return Ok();
        }
    }
}
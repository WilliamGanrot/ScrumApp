using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumApp.Data;
using ScrumApp.Models;

namespace ScrumApp.Controllers
{
    public class BoardController : Controller
    {
        private readonly ScrumApplicationContext context;
        private readonly UserManager<AppUser> userManager;

        public BoardController(ScrumApplicationContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        /*
        när man klickar på ett projekt kommer man till det senaste "boardet",
        men man kan även i en undermeny klicka i föregående boards, samt skapa ett nytt 

        Index should redirect to the latest created board, if no board has been created yet dont redirect,
        view message that displays no board created yet.
        */
        public async Task<IActionResult> Index(string userSlug, string projectSlug)
        {

            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            //check if the user exists
            AppUser projectOwner = context.Users
                .Where(x => x.UserName.ToLower().Replace(" ", "-") == userSlug)
                .FirstOrDefault();
            
            //if (projectOwner == null)
            //    return "Could not find user";

            //check if the projects exists
            var project = context.Projects
                .Where(x => x.ProjectName.ToLower().Replace(" ", "-") == projectSlug)
                .Where(x => x.Author == projectOwner)
                .FirstOrDefault();
            
            if (project == null)
                return NotFound();

            //check if logged in user is a member of the project
            //GIVES ERROR
            var result = context.UserProjects
                .Where(x => x.AppUser == user)
                .Where(x => x.ProjectId == project.ProjectId)
                .FirstOrDefault();

            if (result == null)
                return NotFound();


             var boards = context.Boards
                .Where(x => x.Project == project);

            if (boards.Count() == 0)
            {
                System.Diagnostics.Debug.WriteLine("No boards");
            }
            else
            {
                var latestBoard = boards.OrderByDescending(p => p.BoardId).FirstOrDefault();
                string latestBoardSlug = latestBoard.BoardName.ToLower().Replace(" ", "-");


                //This url will trigger the action Board-action
                var url = "https://localhost:44388/" + project.Author.UserNameSlug + "/" + project.Slug + "/" + latestBoardSlug;
                return Redirect(url);
                //return RedirectToAction("Index", new { userSlug = projectOwner.UserNameSlug, projectSlug = project.Slug, boardSlug = latestBoardSlug });

            }

            return View(project);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBoard createBoard, string userSlug, string projectSlug)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.GetUserAsync(HttpContext.User);

                AppUser projectOwner = context.Users
                    .Where(x => x.UserName.ToLower().Replace(" ", "-") == userSlug)
                    .FirstOrDefault();

                //check if the projects exists
                var project = context.Projects
                    .Where(x => x.ProjectName.ToLower().Replace(" ", "-") == projectSlug)
                    .Where(x => x.Author == projectOwner)
                    .FirstOrDefault();

                if (project == null)
                    return NotFound();

                //check if logged in user is a member of the project
                var result = context.UserProjects
                    .Where(x => x.AppUser == user)
                    .Where(x => x.ProjectId == project.ProjectId)
                    .FirstOrDefault();

                if (result == null)
                    return NotFound();


                string slug = createBoard.BoardName.ToLower().Replace(" ", "-");

                var SlugExist = context.Boards
                    .Where(x => x.BoardSlug == slug);

                if (SlugExist == null)
                {
                    System.Diagnostics.Debug.WriteLine("A board with the title allready exists");
                    return View();
                }

                Board board = new Board
                {
                    BoardName = createBoard.BoardName,
                    BoardSlug = slug,
                    Project = project
                };

                await context.Boards.AddAsync(board);
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }


        public async Task<IActionResult> Board(string userSlug, string projectSlug, string boardSlug)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            //check if the user exists
            AppUser projectOwner = context.Users
                .Where(x => x.UserName.ToLower().Replace(" ", "-") == userSlug)
                .FirstOrDefault();

            //if (projectOwner == null)
            //    return "Could not find user";

            //check if the projects exists
            var project = context.Projects
                .Where(x => x.ProjectName.ToLower().Replace(" ", "-") == projectSlug)
                .Where(x => x.Author == projectOwner)
                .FirstOrDefault();

            if (project == null)
                return NotFound();

            //check if logged in user is a member of the project
            //GIVES ERROR
            var result = context.UserProjects
                .Where(x => x.AppUser == user)
                .Where(x => x.ProjectId == project.ProjectId)
                .FirstOrDefault();

            if (result == null)
                return NotFound();

            var boards = context.Boards
               .Where(x => x.Project == project);
                
            if (boards == null)
                return NotFound();

            Board currentBoard = boards
                .Where(board => board.BoardSlug == boardSlug)
                .Include(board => board.BoardColumns)
                .ThenInclude(column => column.Stories)
                .FirstOrDefault();
            
            //order boardcolumns
            currentBoard.BoardColumns = currentBoard.BoardColumns.OrderBy(c => c.BoardColumnSorting).ToList();
              
            foreach(BoardColumn column in currentBoard.BoardColumns)
                column.Stories = column.Stories.OrderBy(c => c.StorySorting).ToList();


            ViewBag.boards = boards;
            return View(currentBoard);
        }
    }
}
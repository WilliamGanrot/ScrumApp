using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumApp.Data;
using ScrumApp.Models;
using ScrumApp.Services.Board_;

namespace ScrumApp.Controllers
{
    public class BoardController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IBoardService boardService;

        public BoardController(UserManager<AppUser> userManager, IBoardService boardService)
        {
            this.boardService = boardService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string userSlug, string projectSlug)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            AppUser projectOwner = boardService.GetUserBySlug(userSlug);

            Project project = boardService.GetProjectBySlug(projectSlug, projectOwner);
            if (project == null)
                return NotFound();

            if (!boardService.IsMemberOfProject(user, project))
                return NotFound();

            IQueryable<Board> boards = boardService.GetBoards(project);

            if (boards.Count() > 0)
            {
                var latestBoard = boards.OrderByDescending(p => p.BoardId).FirstOrDefault();
                string latestBoardSlug = latestBoard.BoardName.ToLower().Replace(" ", "-");

                var url = "https://localhost:44388/" + project.Author.UserNameSlug + "/" + project.Slug + "/" + latestBoardSlug;
                return Redirect(url);
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
            if (!ModelState.IsValid)
                return View();

            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            AppUser projectOwner = boardService.GetUserBySlug(userSlug);

            Project project = boardService.GetProjectBySlug(projectSlug, projectOwner);
            if (project == null)
                return NotFound();

            if (!boardService.IsMemberOfProject(user, project))
                return NotFound();

            string slug = createBoard.BoardName.ToLower().Replace(" ", "-");

            if (!boardService.ProjectNameIsAvailable(slug))
                return View();

            bool successful = await boardService.CreateBoard(createBoard, project, slug);
            if (!successful)
                return BadRequest("Could not create board");

            return RedirectToAction("Index"); 
        }


        public async Task<IActionResult> Board(string userSlug, string projectSlug, string boardSlug)
        {
            System.Diagnostics.Debug.WriteLine("board");
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            AppUser projectOwner = boardService.GetUserBySlug(userSlug);

            Project project = boardService.GetProjectBySlug(projectSlug, projectOwner);
            if (project == null)
                return NotFound();

            
            if (!boardService.IsMemberOfProject(user, project))
                return NotFound();


            IQueryable<Board> boards = boardService.GetBoards(project);  
            if (boards == null)
                return NotFound();

            Board currentBoard = boardService.GetBoardWithColumnAndStories(boards, boardSlug);

            ViewBag.boards = boards;
            return View(currentBoard);
        }
    }
}
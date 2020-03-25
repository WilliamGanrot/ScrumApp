using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScrumApp.Data;
using ScrumApp.Models;
using ScrumApp.Services.BoardColumn_;

namespace ScrumApp.Controllers
{
    public class BoardColumnController : Controller
    {
        private readonly IBoardColumnService BoardColumnService;
        private readonly UserManager<AppUser> userManager;
        public BoardColumnController(UserManager<AppUser> userManager, IBoardColumnService BoardColumnService)
        {
            this.BoardColumnService = BoardColumnService;
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
        public async Task<IActionResult> Create(BoardColumn boardColumn, string userSlug, string projectSlug, string boardSlug)
        {
            System.Diagnostics.Debug.WriteLine(userSlug + projectSlug + boardSlug + boardColumn.BoardColumnName);
            if (!ModelState.IsValid)
                return RedirectToAction("Board", "Board");
            
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            AppUser projectOwner = BoardColumnService.GetUserBySlug(userSlug);

            Project project = BoardColumnService.GetProjectBySlug(projectSlug, projectOwner);
            if (project == null)
                return NotFound();

            if (!BoardColumnService.IsMemberOfProject(user, project))
                return NotFound();

            IQueryable<Board> boards = BoardColumnService.GetBoards(project);
            Board currentBoard = BoardColumnService.GetBoardBySlug(boards, boardSlug);

            string slug = boardColumn.BoardColumnName.ToLower().Replace(" ", "-");

            if (BoardColumnService.IsBussyBoardColumnSlug(currentBoard, slug))
            {
                TempData["Error"] = "Coulmn with name allready exists";
                return View();
            }

            bool successful = await BoardColumnService.SaveBoardColumn(boardColumn, slug, currentBoard);
            if (!successful)
                return BadRequest("Could not create Column");
            return RedirectToAction("Board", "Board");

        }

        [HttpPost]
        public async Task<IActionResult> reorder(int id, int[] vals)
        {
            bool successful = await BoardColumnService.ReorderColumns(id, vals);
            if (!successful)
                return StatusCode(500);

            return Ok();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using ScrumApp.Data;
using ScrumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Services.Board_
{
    public class BoardService : IBoardService
    {
        private readonly ScrumApplicationContext context;
        public BoardService(ScrumApplicationContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateBoard(CreateBoard createBoard, Project project, string slug)
        {
            Board board = new Board
            {
                BoardName = createBoard.BoardName,
                BoardSlug = slug,
                Project = project,

            };

            await context.Boards.AddAsync(board);
            int saveResult = await context.SaveChangesAsync();

            return saveResult == 1;
        }

        public Board get_partial_view(int b)
        {
            var z = context.Boards.Where(x => x.BoardId == b)
                .Include(board => board.BoardColumns)
                .ThenInclude(column => column.Stories)
                .ThenInclude(story => story.UserStories)
                .FirstOrDefault();

            return z;
        }

            public Board GetBoardWithColumnAndStories(IQueryable<Board> boards, string boardSlug)
        {
            Board currentBoard = boards
                .Where(board => board.BoardSlug == boardSlug)
                .Include(board => board.BoardColumns)
                .ThenInclude(column => column.Stories)
                .ThenInclude(story => story.UserStories)
                .ThenInclude(userStory => userStory.AppUser)
                .FirstOrDefault();

            //order boardcolumns
            currentBoard.BoardColumns = currentBoard.BoardColumns.OrderBy(c => c.BoardColumnSorting).ToList();

            foreach (BoardColumn column in currentBoard.BoardColumns)
                column.Stories = column.Stories.OrderBy(c => c.StorySorting).ToList();

            return currentBoard;
        }

        public IQueryable<Board> GetBoards(Project project)
        {
            IQueryable<Board> boards = context.Boards
                .Where(x => x.Project == project);

            return boards;
        }

        public Project GetProjectBySlug(string slug, AppUser projectOwner)
        {
            Project project = context.Projects
                .Where(x => x.ProjectName.ToLower().Replace(" ", "-") == slug)
                .Where(x => x.Author == projectOwner)
                .FirstOrDefault();

            return project;
        }

        public AppUser GetUserBySlug(string slug)
        {
            AppUser user = context.Users
                .Where(x => x.UserName.ToLower().Replace(" ", "-") == slug)
                .FirstOrDefault();

            return user;
        }

        public bool IsMemberOfProject(AppUser user, Project project)
        {
            UserProject result = context.UserProjects
                .Where(x => x.AppUser == user)
                .Where(x => x.ProjectId == project.ProjectId)
                .FirstOrDefault();

            if (result == null)
                return false;
            return true;
        }

        public bool ProjectNameIsAvailable(string slug)
        {
            IQueryable<Board> boards = context.Boards
                .Where(x => x.BoardSlug == slug);

            if (boards == null || boards.Count() == 0)
                return true;
            return false;
        }
    }
}

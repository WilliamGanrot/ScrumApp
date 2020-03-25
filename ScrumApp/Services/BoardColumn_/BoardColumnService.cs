using ScrumApp.Data;
using ScrumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Services.BoardColumn_
{
    public class BoardColumnService : IBoardColumnService
    {
        private readonly ScrumApplicationContext context;
        public BoardColumnService(ScrumApplicationContext context)
        {
            this.context = context;
        }
        public async Task<bool> ReorderColumns(int id, int[] vals)
        {
            int boardId = id;
            int[] columnsId = vals;

            int newSorting = 1;

            foreach (var valid in columnsId)
            {
                var column = await context.BoardColumns.FindAsync(valid);
                column.BoardColumnSorting = newSorting;

                context.BoardColumns.Update(column);

                newSorting += 1;
            }
            int saveResult = await context.SaveChangesAsync();

            return saveResult == 1;
        }
        public IQueryable<Board> GetBoards(Project project)
        {
            IQueryable<Board> boards = context.Boards
                .Where(x => x.Project == project);

            return boards;
        }

        public Project GetProjectBySlug(string projectSlug, AppUser projectOwner)
        {
            Project project = context.Projects
                .Where(x => x.ProjectName.ToLower().Replace(" ", "-") == projectSlug)
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

        public Board GetBoardBySlug(IQueryable<Board> projectBoards, string boardSlug)
        {
            Board currentBoard = projectBoards
                .Where(x => x.BoardSlug == boardSlug)
                .FirstOrDefault();

            return currentBoard;
            
        }

        public bool IsBussyBoardColumnSlug(Board currentBoard, string slug)
        {
            IEnumerable<BoardColumn> SlugExist = context.BoardColumns
                .Where(x => x.Board == currentBoard)
                .Where(x => x.BoardColumnSlug == slug);

            if (SlugExist == null || SlugExist.Count() == 0)
                return false;
            return true;


        }

        public async Task<bool> SaveBoardColumn(BoardColumn boardColumn, string slug, Board currentBoard)
        {
            boardColumn.BoardColumnSlug = slug;
            boardColumn.Board = currentBoard;
            boardColumn.BoardColumnSorting = 100;

            await context.BoardColumns.AddAsync(boardColumn);
            int saveResult = await context.SaveChangesAsync();

            return saveResult == 1;
        }
    }
}

using ScrumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Services.BoardColumn_
{
    public interface IBoardColumnService
    {
        AppUser GetUserBySlug(string slug);
        Project GetProjectBySlug(string projectSlug, AppUser projectOwner);
        bool IsMemberOfProject(AppUser user, Project project);
        IQueryable<Board> GetBoards(Project project);
        Board GetBoardBySlug(IQueryable<Board> projectBoards, string boardSlug);
        Task<bool> ReorderColumns(int id, int[] vals);
        Task<bool> SaveBoardColumn(BoardColumn boardColumn, string slug, Board currentBoard);
        bool IsBussyBoardColumnSlug(Board currentBoard, string slug);
    }
}

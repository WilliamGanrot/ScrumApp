using ScrumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Services.Board_
{
    public interface IBoardService
    {
        AppUser GetUserBySlug(string slug);
        Project GetProjectBySlug(string slug, AppUser projectOwner);
        bool IsMemberOfProject(AppUser user, Project project);
        IQueryable<Board> GetBoards(Project project);
        bool ProjectNameIsAvailable(string slug);
        Task<bool> CreateBoard(CreateBoard createBoard, Project project, string slug);
        Board GetBoardWithColumnAndStories(IQueryable<Board> boards, string boardSlug);
        Board get_partial_view(int b);
    }
}

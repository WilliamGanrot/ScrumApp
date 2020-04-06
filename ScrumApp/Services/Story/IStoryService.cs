using ScrumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Services
{
    public interface IStoryService
    {
        Task<bool> Create(CreateStory createStory);
        Task<bool> Reorder(int BoardColumnId, int[] vals);
        Task<bool> Delete(int id);
        Task<bool> AssignToStory(int id, AppUser user);
        Task<bool> DissociateToStory(int id, AppUser user);
    }
}

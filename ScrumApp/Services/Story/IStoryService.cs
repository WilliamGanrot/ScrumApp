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
    }
}

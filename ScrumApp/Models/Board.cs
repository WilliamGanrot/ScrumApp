using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class Board
    {
        public string BoardName { get; set; }
        public int BoardId { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}

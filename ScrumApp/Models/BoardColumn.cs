using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class BoardColumn
    {
        public string BoardColumnName { get; set; }
        public string BoardColumnSlug { get; set; }
        public int BoardColumnId { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }

        public ICollection<Story> Stories { get; set; }

    }
}

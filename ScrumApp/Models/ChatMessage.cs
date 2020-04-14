using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class ChatMessage
    {
        public int MessageId { get; set; }
        public string MessageText { get; set; }
        public DateTime TimeSent { get;  set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public string AuthorId { get; set; }
        public virtual AppUser Author { get; set; }
    }
}

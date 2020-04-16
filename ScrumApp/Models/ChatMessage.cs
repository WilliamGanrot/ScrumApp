using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models
{
    public class ChatMessage
    {
        [Key]
        public int ChatMessageId { get; set; }
        public string MessageText { get; set; }
        public DateTime TimeSent { get;  set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public string AuthorId { get; set; }
        
        [ForeignKey("AuthorId")]
        public AppUser Author { get; set; }
    }
}

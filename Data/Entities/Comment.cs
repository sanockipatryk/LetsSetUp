using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetsSetUp.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public DateTime CommentDate { get; set; }

        public string AuthorName { get; set; }

        public bool IsCreator { get; set; }

        [Required]
        public string Content { get; set; }

        public AppUser AppUser { get; set; }

        public Event Event { get; set; }
    }
}

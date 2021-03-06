﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetsSetUp.Data.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public DateTime DateSent { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public bool IsRead { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int UserId { get; set; }

        public Event Event { get; set; }

        public AppUser AppUser { get; set; }
    }
}

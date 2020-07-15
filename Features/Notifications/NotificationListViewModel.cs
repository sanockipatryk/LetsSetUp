using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsSetUp.Features.Notifications
{
    public class NotificationListViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime DateSent { get; set; }

        public bool IsRead { get; set; }

        public int EventId { get; set; }

        public string Slug { get; set; }

    }
}

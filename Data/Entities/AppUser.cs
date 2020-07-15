using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LetsSetUp.Data.Entities
{

    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public List<Event> Events { get; set; }
        public List<Comment> Comments { get; set; }
        public List<SignUp> SignUps { get; set; }
        public List<Notification> Notifications { get; set; }
    }

}

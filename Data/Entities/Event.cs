using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LetsSetUp.Data.Entities
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateStarts { get; set; }

        [Required]
        public DateTime DateEnds { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public int PeopleNeeded { get; set; }

        [Required]
        public string City { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }

        public string ObjectName { get; set; }

        public string PlaceDescription { get; set; }

        [Required]
        public string EventDescription { get; set; }

        public string WebPage { get; set; }

        public bool IsCancelled { get; set; }

        public Category Category { get; set; }

        public AppUser AppUser { get; set; }

        public List<SignUp> SignUps { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Notification> Notifications { get; set; }

    }
}

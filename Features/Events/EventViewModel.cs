﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsSetUp.Features.Events
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateStarts { get; set; }
        public DateTime DateEnds { get; set; }
        public int PeopleNeeded { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ObjectName { get; set; }
        public string PlaceDescription { get; set; }
        public string EventDescription { get; set; }
        public string WebPage { get; set; }
        public bool IsCancelled { get; set; }
        public int UsersSigned { get; set; }

        public int CreatedBy { get; set; }
        public string Category { get; set; }
        public string Status
        {
            get
            {
                if (DateStarts > DateTime.Now)
                    return "Nadchodzące";
                else if (DateStarts <= DateTime.Now && DateEnds > DateTime.Now)
                    return "W trakcie";
                else
                    return "Zakończone";
            }
        }
    }
}

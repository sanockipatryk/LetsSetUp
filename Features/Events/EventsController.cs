using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsSetUp.Data;
using LetsSetUp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LetsSetUp.Data.Entities;

namespace LetsSetUp.Features.Events
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly LetsSetUpContext _db;

        public EventsController(LetsSetUpContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Find(string categories, string statuses, DateTime? dateStarts, DateTime? dateEnds, string name, string city, bool mine)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var Categories = string.IsNullOrEmpty(categories) ? new List<string>() : categories.Split('|').ToList();
            var Statuses = string.IsNullOrEmpty(statuses) ? new List<string>() : statuses.Split('|').ToList();

            var events = await _db.Events
                .Where(x => Categories.Any() == false || Categories.Contains(x.Category.Name))
                .Where(x => Statuses.Contains("Nadchodzące") == false || x.DateStarts > DateTime.Now)
                .Where(x => Statuses.Contains("W trakcie") == false || x.DateStarts <= DateTime.Now && x.DateEnds>DateTime.Now)
                .Where(x => Statuses.Contains("Zakończone") == false || x.DateEnds <= DateTime.Now)
                .Where(x => dateStarts == null || x.DateStarts >= dateStarts.Value)
                .Where(x => dateEnds == null || x.DateStarts <= dateEnds.Value)
                .Where(x => name == null || x.Name.ToLower().Contains(name.ToLower()))
                .Where(x => city == null || x.City.ToLower().Contains(city.ToLower()))
                .Where(x => mine == false || x.CreatedBy == appUser.Id)
                .Where(x => x.IsCancelled == false)

                .Select(x => new EventListViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    DateCreated = x.DateCreated,
                    DateStarts = x.DateStarts,
                    DateEnds = x.DateEnds,
                    PeopleNeeded = x.PeopleNeeded,
                    City = x.City,
                    Street = x.Street,
                    BuildingNumber = x.BuildingNumber,
                    ObjectName = x.ObjectName,
                    PlaceDescription = x.PlaceDescription,
                    EventDescription = x.EventDescription,
                    WebPage = x.WebPage,
                    IsCancelled = x.IsCancelled,
                    UsersSigned = x.SignUps.Count(),
                    Category = x.Category.Name
                }).ToListAsync();

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var events = await _db.Events.Select(x => new EventViewModel {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                DateCreated = x.DateCreated,
                DateStarts = x.DateStarts,
                DateEnds = x.DateEnds,
                PeopleNeeded = x.PeopleNeeded,
                City = x.City,
                Street = x.Street,
                BuildingNumber = x.BuildingNumber,
                ObjectName = x.ObjectName,
                PlaceDescription = x.PlaceDescription,
                EventDescription = x.EventDescription,
                WebPage = x.WebPage,
                IsCancelled = x.IsCancelled,
                UsersSigned = x.SignUps.Count(),
                Category = x.Category.Name,
                CreatedBy = x.CreatedBy
                
            }).SingleOrDefaultAsync(x => x.Id == id);
            if (events == null)
                return NotFound();

            return Ok(events);
        }

        [HttpPut("cancel/{id}"), Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Cancel(int id)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var currentEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (User.IsInRole("Admin") || appUser.Id == currentEvent.CreatedBy)
            {
                if (currentEvent != null)
                {
                    currentEvent.IsCancelled = !currentEvent.IsCancelled;

                    _db.Events.Update(currentEvent);

                    await _db.SaveChangesAsync();

                    return Ok();
                }

                else
                {
                    return NotFound();
                }
            }
            else
                return Forbid();
        }

        [HttpPut("edit/{id}"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateEventViewModel model)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.Name == model.Category);
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var currentEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (currentEvent != null && currentEvent.DateStarts >= DateTime.Now)
            {
                currentEvent.Name = model.Name;
                currentEvent.DateStarts = model.DateStarts;
                currentEvent.DateEnds = model.DateEnds;
                currentEvent.PeopleNeeded = model.PeopleNeeded;
                currentEvent.City = model.City;
                currentEvent.Street = model.Street;
                currentEvent.BuildingNumber = model.BuildingNumber;
                currentEvent.ObjectName = model.ObjectName;
                currentEvent.PlaceDescription = model.PlaceDescription;
                currentEvent.EventDescription = model.EventDescription;
                currentEvent.WebPage = model.WebPage;
                currentEvent.Category = category;
                currentEvent.CategoryId = category.Id;

                _db.Events.Update(currentEvent);

                await _db.SaveChangesAsync();

                return Ok();
            }
            else
                return Forbid();

        }

        [HttpPost, Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([FromBody] CreateEventViewModel model)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.Name == model.Category);
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var newEvent = new Event
            {
                Name = model.Name,
                Slug = model.Name.GenerateSlug(),
                DateCreated = DateTime.Now,
                DateStarts = model.DateStarts,
                DateEnds = model.DateEnds,
                PeopleNeeded = model.PeopleNeeded,
                City = model.City,
                Street = model.Street,
                BuildingNumber = model.BuildingNumber,
                ObjectName = model.ObjectName,
                PlaceDescription = model.PlaceDescription,
                EventDescription = model.EventDescription,
                WebPage = model.WebPage,
                IsCancelled = false,
                Category = category,
                CreatedBy = appUser.Id,
                CategoryId = category.Id
            };

            _db.Events.Add(newEvent);

            await _db.SaveChangesAsync();

            return Ok();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsSetUp.Data;
using LetsSetUp.Data.Entities;
using LetsSetUp.Features.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsSetUp.Features.Notifications
{
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {
        private readonly LetsSetUpContext _db;
        public NotificationsController(LetsSetUpContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var notifications = await _db.Notifications.OrderByDescending(x => x.DateSent).ToListAsync();
            return Ok(notifications);
        }

        [HttpGet("getNotifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var Events = await _db.Events.ToListAsync();
            var notifications = await _db.Notifications.Where(x => x.UserId == appUser.Id).Where(x => x.IsRead == false).Select(x => new NotificationListViewModel
            {
                Id = x.Id,
                Content = x.Content,
                DateSent = x.DateSent,
                IsRead = x.IsRead,
                EventId = x.EventId,
                Slug = Events.Where(c => c.Id == x.EventId).SingleOrDefault().Slug
            }).OrderByDescending(x => x.DateSent).ToListAsync();
            return Ok(notifications);
        }

        [HttpGet("getAllNotifications")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var Events = await _db.Events.ToListAsync();
            var notifications = await _db.Notifications.Where(x => x.UserId == appUser.Id).Select(x => new NotificationListViewModel
            {
                Id = x.Id,
                Content = x.Content,
                DateSent = x.DateSent,
                IsRead = x.IsRead,
                EventId = x.EventId,
                Slug = Events.Where(c => c.Id == x.EventId).SingleOrDefault().Slug

            }).OrderByDescending(x => x.DateSent).ToListAsync();
            return Ok(notifications);
        }

        [HttpPut("readNotification/{id}"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> ReadNotification(int id)
        {
            var currentNotification = await _db.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            if (currentNotification != null)
            {
                currentNotification.IsRead = !currentNotification.IsRead;

                _db.Notifications.Update(currentNotification);

                await _db.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("notifyCanceled/{id}"), Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> NotifyCancelled(int id)
        {
            var currentEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            var signUps = await _db.SignUps.Where(x => x.EventId == id).ToListAsync();
            
            foreach(var signup in signUps)
            {
                var newNotification = new Notification
                {
                    DateSent = DateTime.Now,
                    IsRead = false,
                    Content = "Wydarzenie \"" + currentEvent.Name + "\" zostało anulowane.",
                    EventId = id,
                    UserId = signup.UserId,
                    AppUser = _db.Users.FirstOrDefault(x => x.Id == signup.UserId),
                    Event = currentEvent
                };
                _db.Notifications.Add(newNotification);
            }
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("notifyEdited/{id}"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> NotifyEdited(int id)
        {
            var currentEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            var signUps = await _db.SignUps.Where(x => x.EventId == id).ToListAsync();

            foreach (var signup in signUps)
            {
                var newNotification = new Notification
                {
                    DateSent = DateTime.Now,
                    IsRead = false,
                    Content = "Wydarzenie \"" + currentEvent.Name + "\" zostało zmienione.",
                    EventId = id,
                    UserId = signup.UserId,
                    AppUser = _db.Users.FirstOrDefault(x => x.Id == signup.UserId),
                    Event = currentEvent
                };
                _db.Notifications.Add(newNotification);
            }
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("notifyNewComment/{id}/{commentAuthorId}"), Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> NotifyNewComment(int id, int commentAuthorId)
        {
            var currentEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            var signUps = await _db.SignUps.Where(x => x.EventId == id).ToListAsync();

            foreach (var signup in signUps)
            {
                if(signup.UserId != commentAuthorId) { 
                var newNotification = new Notification
                {
                    DateSent = DateTime.Now,
                    IsRead = false,
                    Content = "Nowy komentarz do wydarzenia \"" + currentEvent.Name + "\"",
                    EventId = id,
                    UserId = signup.UserId,
                    AppUser = _db.Users.FirstOrDefault(x => x.Id == signup.UserId),
                    Event = currentEvent
                };
                _db.Notifications.Add(newNotification);
                }
            }

            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            if (commentAuthorId != currentEvent.CreatedBy) { 
            var notifyCreator = new Notification
            {
                DateSent = DateTime.Now,
                IsRead = false,
                Content = "Nowy komentarz dla wydarzenia \"" + currentEvent.Name + "\" .",
                EventId = id,
                UserId = currentEvent.CreatedBy,
                AppUser = _db.Users.FirstOrDefault(x => x.Id == currentEvent.CreatedBy),
                Event = currentEvent
            };
            _db.Notifications.Add(notifyCreator);
            }
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("clearNotifications"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> ClearNotifications()
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var notifications = await _db.Notifications.Where(x => x.UserId == appUser.Id).ToListAsync();
            foreach (var notification in notifications)
            {
                _db.Notifications.Remove(notification);
            }
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
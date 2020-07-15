using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsSetUp.Data;
using LetsSetUp.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsSetUp.Features.SignUps
{
    [Route("api/[controller]")]
    public class SignUpsController : Controller
    {
        private readonly LetsSetUpContext _db;
        public SignUpsController(LetsSetUpContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var signups = await _db.SignUps.ToListAsync();
            return Ok(signups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var signups = await _db.SignUps.SingleOrDefaultAsync(x => x.Id == id);
            if (signups == null)
                return NotFound();

            return Ok(signups);
        }

        [HttpGet("isSigned/{id}")]
        public async Task<IActionResult> IsSigned(int id)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            if (appUser == null)
                return Ok(0);
            var signups = await _db.SignUps.Where(x => x.EventId == id).Where(x => x.UserId == appUser.Id).ToListAsync();
            if (signups == null)
                return NotFound();
            else if (signups.Count == 0)
                return Ok(0);
            else
                return Ok(1);
        }

        [HttpPost("signup/{id}"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> SignUp(int id)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var theEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            var signups = await _db.SignUps.Where(x => x.EventId == id).ToListAsync();

            if (signups.Count < theEvent.PeopleNeeded)
            {
                var signUp = new SignUp
                {
                    DateSigned = DateTime.Now,
                    EventId = id,
                    Event = theEvent,
                    UserId = appUser.Id,
                    AppUser = appUser
                };

                _db.SignUps.Add(signUp);
                await _db.SaveChangesAsync();

                return Ok();
            }
            else
                return Forbid();
        }
    

    [HttpDelete("signout/{id}"), Authorize(Roles = "Customer")]
    public async Task<IActionResult> SignOut(int id)
    {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var currentSign = await _db.SignUps.FirstOrDefaultAsync(x => x.EventId == id && x.UserId == appUser.Id);
            if (currentSign == null)
                return NotFound();

            _db.SignUps.Remove(currentSign);

            await _db.SaveChangesAsync();

            return Ok();
    }
    }
}
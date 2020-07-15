using LetsSetUp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsSetUp.Features.Users
{

    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly LetsSetUpContext _db;
        public UsersController(LetsSetUpContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _db.Users.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByName(string userName)
        {
            return Ok(await _db.Users.FirstOrDefaultAsync(x => x.Email == userName));
        }
    }
}

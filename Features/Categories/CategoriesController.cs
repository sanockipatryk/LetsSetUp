using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsSetUp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsSetUp.Features.Categories
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly LetsSetUpContext _db;
        public CategoriesController(LetsSetUpContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var categories = await _db.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var categories = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (categories == null)
                return NotFound();

            return Ok(categories);
        }
    }
}
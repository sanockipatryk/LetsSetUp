using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsSetUp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsSetUp.Features.Filters
{
    [Route("api/[controller]")]
    public class FiltersController : Controller
    {
        private readonly LetsSetUpContext _db;

        public FiltersController(LetsSetUpContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _db.Categories
            .Select(x => x.Name)
            .ToListAsync();
            string[] statuses = {"Nadchodzące", "W trakcie", "Zakończone" };
            return Ok(new FiltersListViewModel
            {
                Categories = categories,
                Statuses = statuses
            });
        }

    }
}
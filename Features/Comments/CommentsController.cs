using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsSetUp.Data;
using LetsSetUp.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsSetUp.Features.Comments
{
    [Route("api/[controller]")]
    public class CommentsController : Controller
    {
        private readonly LetsSetUpContext _db;
        public CommentsController(LetsSetUpContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var comments = await _db.Comments.OrderByDescending(x => x.CommentDate).ToListAsync();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var comments = await _db.Comments.Where(x => x.EventId == id).OrderByDescending(x => x.CommentDate).ToListAsync();
            if (comments == null)
                return NotFound();

            return Ok(comments);
        }

        [HttpPost("addComment/{id}"), Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> AddComment(int id, [FromBody] CreateCommentViewModel model)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var theEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            var isCreator = appUser.Id == theEvent.CreatedBy;

            var comment = new Comment
            {
                CommentDate = DateTime.Now,
                EventId = id,
                Event = theEvent,
                UserId = appUser.Id,
                AuthorName = appUser.FirstName,
                IsCreator = isCreator,
                AppUser = appUser,
                Content = model.Content
            };

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("deleteComment/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var currentComment = await _db.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (currentComment == null)
                return NotFound();

            _db.Comments.Remove(currentComment);

            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
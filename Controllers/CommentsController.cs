using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtMapApi.Data;
using ArtMapApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtMapApi.Controllers
{
    [Produces("application/json")]
    [Route("/api/comments")]
    // [EnableCors("AllowSpecificOrigin")]
    public class CommentsController : Controller
    {
        private ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        // GET /comments
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<object> comments = from comment in _context.Comment select comment;

            if (comments == null)
            {
                return NotFound();
            }

            return Ok(comments);

        }

        // GET /comments/5
        [HttpGet("{id}", Name = "GetComment")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Comment comment = _context.Comment.Single(m => m.CommentId == id);

                if (comment == null)
                {
                    return NotFound();
                }

                return Ok(comment);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }


        }

        // POST /comments
        [HttpPost]
        public IActionResult Comment([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Comment.Add(comment);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CommentExists(comment.CommentId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetComment", new { id = comment.CommentId }, comment);
        }

        // PUT /comments/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.CommentId)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        // DELETE /comments/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Comment comment = _context.Comment.Single(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comment.Remove(comment);
            _context.SaveChanges();

            return Ok(comment);
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Count(e => e.CommentId == id) > 0;
        }

    }
}
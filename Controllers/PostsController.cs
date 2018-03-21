using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArtMapApi.Data;
using ArtMapApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtMapApi.Controllers
{
    [Produces("application/json")]
    [Route("/api/posts")]
    // [EnableCors("AllowSpecificOrigin")]
    public class PostsController : Controller
    {
        private ApplicationDbContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public PostsController(ApplicationDbContext ctx, IHostingEnvironment environment)
        {
            _context = ctx;
            _hostingEnvironment = environment;
        }

        // GET /posts
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<object> posts = from post in _context.Post select post;

            if (posts == null)
            {
                return NotFound();
            }

            return Ok(posts);

        }

        // GET /posts/5
        [HttpGet("{id}", Name = "GetPost")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Post post = _context.Post.Single(m => m.PostId == id);

                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }


        }

        // POST /posts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //autopopulate datecreated with the current time
            post.CreatedAt = DateTime.Now;

            //add it all to the db
            _context.Post.Add(post);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PostExists(post.PostId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetPost", new { id = post.PostId }, post);
        }

        // PUT /posts/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != post.PostId)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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

        // DELETE /posts/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Post post = _context.Post.Single(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Post.Remove(post);
            _context.SaveChanges();

            return Ok(post);
        }

        private bool PostExists(int id)
        {
            return _context.Post.Count(e => e.PostId == id) > 0;
        }

    }
}
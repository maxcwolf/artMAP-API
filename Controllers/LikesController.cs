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
    [Route("api/likes")]
    // [EnableCors("AllowSpecificOrigin")]
    public class LikesController : Controller
    {
        private ApplicationDbContext _context;

        public LikesController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        // GET /likes
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<object> likes = from like in _context.Like select like;

            if (likes == null)
            {
                return NotFound();
            }

            return Ok(likes);

        }

        // GET /likes/5
        [HttpGet("{id}", Name = "GetLike")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Like like = _context.Like.Single(m => m.LikeId == id);

                if (like == null)
                {
                    return NotFound();
                }

                return Ok(like);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }


        }

        // POST /likes
        [HttpPost]
        public IActionResult Like([FromBody] Like like)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Like.Add(like);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LikeExists(like.LikeId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetLike", new { id = like.LikeId }, like);
        }

        // PUT /likes/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Like like)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != like.LikeId)
            {
                return BadRequest();
            }

            _context.Entry(like).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeExists(id))
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

        // DELETE /likes/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Like like = _context.Like.Single(m => m.LikeId == id);
            if (like == null)
            {
                return NotFound();
            }

            _context.Like.Remove(like);
            _context.SaveChanges();

            return Ok(like);
        }

        private bool LikeExists(int id)
        {
            return _context.Like.Count(e => e.LikeId == id) > 0;
        }

    }
}
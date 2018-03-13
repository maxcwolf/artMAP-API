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
    [Route("users")]
    [EnableCors("AllowSpecificOrigin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext _context;

        public UsersController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        // GET all /users
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<object> users = from user in _context.User select user;

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);

        }

        // GET /users/5
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                User user = _context.User.Single(m => m.UserId == id);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }


        }

        // POST /users
        [HttpPost]
        public IActionResult User([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.User.Add(user);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetUser", new { id = user.UserId }, user);
        }

        // PUT /users/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // DELETE /users/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _context.User.Single(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            _context.SaveChanges();

            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _context.User.Count(e => e.UserId == id) > 0;
        }

    }
}
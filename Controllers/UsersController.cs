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
    [Route("/api/users")]
    // [EnableCors("AllowSpecificOrigin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext _context;

        public UsersController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }


        // GET /users/5
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                User user = _context.User.Single(m => m.Id == id);

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

        private bool UserExists(string id)
        {
            return _context.User.Count(e => e.Id == id) > 0;
        }

    }
}
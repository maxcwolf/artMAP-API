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

    [Route("/api/posts")]

    public class PhotosController : Controller
    {
        // private ApplicationDbContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public PhotosController(IHostingEnvironment environment)
        {
            // _context = ctx;
            _hostingEnvironment = environment;

        }


        // POST /posts
        [HttpPost("upload")]
        public async Task<IActionResult> SubmitImageData([FromBody] Photo photo)
        {

            //specify the filepath
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "images");

            string imageName = photo.ImgName + ".jpg";

            //set the image path
            string imgPath = Path.Combine(path, imageName);

            byte[] imageBytes = Convert.FromBase64String(photo.ImgStr);

            System.IO.File.WriteAllBytes(imgPath, imageBytes);

            return Ok(new { imgPath });
        }

    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;

namespace RozetkaWebApp.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Images
        [HttpGet("[controller]/{id}")]
        public async Task<FileResult> Item(long? id)
        {
            if (id == null) return null;
            var image = await _context.Image.FirstOrDefaultAsync(m => m.ImageId == id);
            if (image == null) return null;
            return image.ToStream();
            //System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(image.Data);
            //return new FileStreamResult(oMemoryStream, "image/*");
        }


        public async Task<FileResult> Index(long? id)
        {
            if (id == null) return null;
            var image = await _context.Image.FirstOrDefaultAsync(m => m.ImageId == id);
            if (image == null) return null;
            return image.ToStream();
            //System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(image.Data);
            //return new FileStreamResult(oMemoryStream, "image/*");
        }

    }     
}

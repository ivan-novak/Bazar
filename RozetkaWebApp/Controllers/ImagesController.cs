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
            System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(image.Data);
            var ext = Path.GetExtension(image.Title).Substring(1);
            return new FileStreamResult(oMemoryStream, "image/" + ext);
        }


        public async Task<FileResult> Index(long? id)
        {
            if (id == null) return null;
            var image = await _context.Image.FirstOrDefaultAsync(m => m.ImageId == id);
            if (image == null) return null;
          //  return image.ToStream();
            if (image.Data == null) return null;
            System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(image.Data);
            var ext = Path.GetExtension(image.Title).Substring(1);
            return new FileStreamResult(oMemoryStream, "image/" + ext);
        }

        [HttpGet("[controller]/product/{id}/{name}")]
        public async Task<FileResult> Product(long? id, string name)
        {
            if (id == null || name == null) return null;
            var productImage = await _context.ProductImage.Where(p => p.ProductId == id && p.Label == name).FirstOrDefaultAsync();
            if (productImage == null) return null;
            return await Index(productImage.ImageId);
        }

        [HttpGet("[controller]/catalog/{id}/{name}")]
        public async Task<FileResult> Catalog(long? id, string name)
        {
            if (id == null || name == null) return null;
            var catalogImage = await _context.CatalogImage.Where(p => p.CatalogId == id && p.Label == name).FirstOrDefaultAsync();
            if (catalogImage == null) return null;
           return await Index(catalogImage.ImageId);

        }

        [HttpGet("[controller]/portal/{id}/{name}")]
        public async Task<FileResult> Portal(long? id, string name)
        {
            if (id == null || name == null) return null;
            var portalImage = await _context.PortalImage.Where(p => p.PortalId == id && p.Label == name).FirstOrDefaultAsync();
            if (portalImage == null) return null;
            return await Index(portalImage.ImageId);

        }

        [HttpGet("[controller]/root/{name}")]
        public async Task<FileResult> Root(string name)
        {
            var rootImage = await _context.RootImage.Where(p.Label == name).FirstOrDefaultAsync();
            if (rootImage == null) return null;
            return await Index(rootImage.ImageId);

        }

    }     
}

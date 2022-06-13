//MLHIDEFILE
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;

namespace RozetkaWebApp.Controllers
{
    [Authorize(Roles = "Користувачі")]
    public class ImagesController : Controller
    {
        private readonly RozetkadbContext _context;

        public ImagesController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Images
        [HttpGet("[controller]/{id}")]
        [HttpGet("[controller]/v1/{id}")]
        public async Task<FileResult> Item(long? id)
        {
            if (id == null) return null;
            var image = await _context.Images.FirstOrDefaultAsync(m => m.ImageId == id);
            if (image == null) return null;
        //    return Images.ToStream();
            System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(image.Data);
            var ext = Path.GetExtension(image.Title).Substring(1).Replace("svg", "svg+xml");
            return new FileStreamResult(oMemoryStream, "image/" + ext);
        }


        public async Task<FileResult> Index(long? id)
        {
            if (id == null) return null;
            var image = await _context.Images.FirstOrDefaultAsync(m => m.ImageId == id);
            if (image == null) return null;
          //  return Images.ToStream();
            if (image.Data == null) return null;
            System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(image.Data);
            var ext = Path.GetExtension(image.Title).Substring(1).Replace("svg", "svg+xml");
            return new FileStreamResult(oMemoryStream, "image/" + ext);
        }

        [HttpGet("[controller]/products/{id}/{name}")]
        public async Task<FileResult> Product(long? id, string name)
        {
            if (id == null || name == null) return null;
            var productImage = await _context.ProductImages.Where(p => p.ProductId == id && p.Label == name).FirstOrDefaultAsync();
            if (productImage == null) return null;
            return await Index(productImage.ImageId);
        }

        [HttpGet("[controller]/catalogs/{id}/{name}")]
        public async Task<FileResult> Catalog(long? id, string name)
        {
            if (id == null || name == null) return null;
            var catalogImage = await _context.CatalogImages.Where(p => p.CatalogId == id && p.Label == name).FirstOrDefaultAsync();
            if (catalogImage == null) return null;
           return await Index(catalogImage.ImageId);

        }

        [HttpGet("[controller]/portals/{id}/{name}")]
        public async Task<FileResult> Portal(long? id, string name)
        {
            if (id == null || name == null) return null;
            var portalImage = await _context.PortalImages.Where(p => p.PortalId == id && p.Label == name).FirstOrDefaultAsync();
            if (portalImage == null) return null;
            return await Index(portalImage.ImageId);

        }

        [HttpGet("[controller]/root/{name}")]
        public async Task<FileResult> Root(string name)
        {
            var rootImage = await _context.RootImages.Where(p => p.Label == name).FirstOrDefaultAsync();
            if (rootImage == null) return null;
            return await Index(rootImage.ImageId);

        }

    }     
}

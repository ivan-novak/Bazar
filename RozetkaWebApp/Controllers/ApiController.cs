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
    public class ApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Portals
        [HttpGet("[controller]/portals")]
        public async Task<ActionResult<IEnumerable<Portal>>> Portals(string orderBy = "PortalId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, int deep = 0)
        {
            var query = _context.Portal.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.PortalId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page*pageSize).Take(pageSize);
            if(deep > 0) query = query.Include(x=>x.Catalogs);
            return await query.ToListAsync();

        }

        // GET: Portal One
        [HttpGet("[controller]/portals/{portalId}")]
        public async Task<ActionResult<Portal>> Portals(int portalId)      
        {
            // var portal = await _context.Portal.Include(x=>x.Catalogs).FirstOrDefaultAsync(x => x.PortalId == portalId);
            var portal = await _context.Portal.FindAsync(portalId); 
            if (portal == null) return NotFound();
            return portal;
        }

        [HttpGet("[controller]/portals/{portalId}/catalogs/")]
        [HttpGet("[controller]/catalogs")]
        public async Task<ActionResult<IEnumerable<Catalog>>> Catalogs(string orderBy = "CatalogId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? portalId = null)
        {
            var query = _context.Catalog.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (portalId != null) query = query.Where(x => x.PortalId == portalId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.CatalogId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.ToListAsync();

        }

        // GET: Portal One
        [HttpGet("[controller]/catalogs/{catalogId}")]
        public async Task<ActionResult<Catalog>> Catalogs(int catalogId)
        {
            var catalog = await _context.Catalog.FindAsync(catalogId);
            if (catalog == null) return NotFound();
            return catalog;
        }

        [HttpGet("[controller]/catalogs/{catalogId}/products/")]
        [HttpGet("[controller]/products")]
        public async Task<ActionResult<IEnumerable<Product>>> Products(string orderBy = "ProductId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? catalogId = null)
        {
            var query = _context.Product.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else if (orderBy == "PRICE") query = query.OrderBy(x => x.Price);
            else query = query.OrderBy(x => x.ProductId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.ToListAsync();

        }

        // GET: Portal One
        [HttpGet("[controller]/products/{productId}")]
        public async Task<ActionResult<Product>> Products(long productId)
        {
            var item = await _context.Product.FindAsync(productId);
            if (item == null) return NotFound();
            return item;
        }

          [HttpGet("[controller]/catalogs/{catalogId}/images/")]
        public async Task<ActionResult<IEnumerable<CatalogImage>>> CatalogImages(string orderBy = "CatalogImageId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? catalogId = null)
        {
            var query = _context.CatalogImage.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.Id);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }

        [HttpGet("[controller]/portals/{portalId}/Images/")]
        public async Task<ActionResult<IEnumerable<PortalImage>>> PortalImages(string orderBy = "PortalImagesId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? portalId = null)
        {
            var query = _context.PortalImage.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (portalId != null) query = query.Where(x => x.PortalId == portalId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.PortalImageId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }



        [HttpGet("[controller]/catalogs/{catalogId}/properties/")]
        [HttpGet("[controller]/properties")]
        public async Task<ActionResult<IEnumerable<Property>>> Properties(string orderBy = "PropertyId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? catalogId = null)
        {
            var query = _context.Property.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.PropertyId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.ToListAsync();

        }

        // GET: Portal One
        [HttpGet("[controller]/propereties/{propertyId}")]
        public async Task<ActionResult<Property>> Properties(int propertyId)
        {
            var item = await _context.Property.FindAsync(propertyId);
            if (item == null) return NotFound();
            return item;
        }


        [HttpGet("[controller]/products/{productId}/images/")]
        public async Task<ActionResult<IEnumerable<ProductImage>>> ProductImages(string orderBy = "ProductImageId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? productId = null)
        {
            var query = _context.ProductImage.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (productId != null) query = query.Where(x => x.ProductId == productId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.ProductImageId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }


        [HttpGet("[controller]/prodcuts/{productsId}/characteristics/")]
        [HttpGet("[controller]/characteristics")]
        public async Task<ActionResult<IEnumerable<Characteristic>>> Characteristics(string orderBy = "CharacteristicId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? productId = null)
        {
            var query = _context.Characteristic.Include(x=>x.Property).Select(x => x);
            orderBy = orderBy.ToUpper();
            if (productId != null) query = query.Where(x => x.ProductId == productId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Property.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Property.Title);
            else query = query.OrderBy(x => x.ProductId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.ToListAsync();

        }

        // GET: Portal One
        [HttpGet("[controller]/characteristics/{characteristicId}")]
        public async Task<ActionResult<Product>> Characteristics(int characteristicId)
        {
            var item = await _context.Product.FindAsync(characteristicId);
            if (item == null) return NotFound();
            return item;
        }



    }     
}

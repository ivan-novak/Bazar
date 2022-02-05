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


        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Portals
        [HttpGet("[controller]/v1/portals")]
        public async Task<ActionResult<IEnumerable<Portal>>> Portals(string orderBy = "PortalId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, int deep = 0)
        {
            var query = _context.Portals.Select(x => x);
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
        [HttpGet("[controller]/v1/portals/{portalId}")]
        public async Task<ActionResult<Portal>> Portals(int portalId)      
        {
            // var portal = await _context.Portals.Include(x=>x.Catalogs).FirstOrDefaultAsync(x => x.PortalId == portalId);
            var portal = await _context.Portals.FindAsync(portalId); 
            if (portal == null) return NotFound();
            return portal;
        }

        [HttpGet("[controller]/v1/portals/{portalId}/catalogs/")]
        [HttpGet("[controller]/v1/catalogs")]
        public async Task<ActionResult<IEnumerable<Catalog>>> Catalogs(string orderBy = "CatalogId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? portalId = null)
        {
            var query = _context.Catalogs.Select(x => x);
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
        [HttpGet("[controller]/v1/catalogs/{catalogId}")]
        public async Task<ActionResult<Catalog>> Catalogs(int catalogId)
        {
            var catalog = await _context.Catalogs.FindAsync(catalogId);
            if (catalog == null) return NotFound();
            return catalog;
        }

        [HttpGet("[controller]/v1/catalogs/{catalogId}/products/")]
        [HttpGet("[controller]/v1/products")]
        public async Task<ActionResult<IEnumerable<Product>>> Products(string orderBy = "ProductId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? catalogId = null)
        {
            var query = _context.Products.Select(x => x);
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
        [HttpGet("[controller]/v1/products/{productId}")]
        public async Task<ActionResult<Product>> Products(long productId)
        {
            var item = await _context.Products.FindAsync(productId);
            if (item == null) return NotFound();
            return item;
        }

        [HttpGet("[controller]/v1/views")]
        [HttpGet("[controller]/v1/portals/{portalId}/views")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/views")]
        [HttpGet("[controller]/v1/users/{userId}/views")]
        public async Task<ActionResult<IEnumerable<Product>>> Products(string orderBy = "ProductId", int page = 0, int pageSize = 50, int? catalogId = null, string userId = null)
        {
            var queryView = _context.Views.Include(x=>x.Product).Select(x => x);
            if (userId != null) queryView = queryView.Where(x => x.UserId == userId);
            if (catalogId != null) queryView = queryView.Where(x => x.Product.CatalogId == catalogId);
            queryView.OrderByDescending(x => x.EventDate);
            return await queryView.Select(x=> x.Product).Distinct().Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        [HttpGet("[controller]/v1/catalogs/{catalogId}/images/")]
        public async Task<ActionResult<IEnumerable<CatalogImage>>> CatalogImages(string orderBy = "CatalogImageId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? catalogId = null)
        {
            var query = _context.CatalogImages.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.Id);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }

        [HttpGet("[controller]/v1/portals/{portalId}/Images/")]
        public async Task<ActionResult<IEnumerable<PortalImage>>> PortalImages(string orderBy = "PortalImagesId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? portalId = null)
        {
            var query = _context.PortalImages.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (portalId != null) query = query.Where(x => x.PortalId == portalId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.PortalImageId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }



        [HttpGet("[controller]/v1/catalogs/{catalogId}/properties/")]
        [HttpGet("[controller]/v1/properties")]
        public async Task<ActionResult<IEnumerable<Property>>> Properties(string orderBy = "PropertyId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? catalogId = null)
        {
            var query = _context.Properties.Select(x => x);
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
        [HttpGet("[controller]/v1/propereties/{propertyId}")]
        public async Task<ActionResult<Property>> Properties(int propertyId)
        {
            var item = await _context.Properties.FindAsync(propertyId);
            if (item == null) return NotFound();
            return item;
        }


        [HttpGet("[controller]/v1/products/{productId}/images/")]
        public async Task<ActionResult<IEnumerable<ProductImage>>> ProductImages(string orderBy = "ProductImageId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? productId = null)
        {
            var query = _context.ProductImages.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (productId != null) query = query.Where(x => x.ProductId == productId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.ProductImageId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }


        [HttpGet("[controller]/v1/prodcuts/{productsId}/characteristics/")]
        [HttpGet("[controller]/v1/characteristics")]
        public async Task<ActionResult<IEnumerable<Characteristic>>> Characteristics(string orderBy = "CharacteristicId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, long? productId = null)
        {
            var query = _context.Characteristics.Include(x=>x.Property).Select(x => x);
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
        [HttpGet("[controller]/v1/characteristics/{characteristicId}")]
        public async Task<ActionResult<Product>> Characteristics(int characteristicId)
        {
            var item = await _context.Products.FindAsync(characteristicId);
            if (item == null) return NotFound();
            return item;
        }

        [HttpGet("[controller]/v1/filters")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/filters/")]
        public async Task<ActionResult<IEnumerable<Filter>>> Filters(string orderBy = "PropertyId", string orderMode = "Desc", int page = 0, int pageSize = 50, int? catalogId = null)
        {
            var query = _context.Filters.Select(x => x);
            if (catalogId != null) query = query.Where(x=>x.CatalogId == catalogId);
            orderBy = orderBy.ToUpper();
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else query = query.OrderBy(x => x.PropertyId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.ToListAsync();

        }


    }     
}

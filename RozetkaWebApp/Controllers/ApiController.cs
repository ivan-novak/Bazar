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
        [HttpGet("[controller]/v1/portals/{portalId}")]
        public async Task<ActionResult<IEnumerable<iPortal>>> Portals(string orderBy = "PortalId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, int? portalId = null)
        {
            var query = _context.Portals.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (portalId != null) query = query.Where(x => x.PortalId == portalId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.PortalId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page*pageSize).Take(pageSize);
            return await query.Select(x=>(iPortal)x).ToListAsync();
        }

        [HttpGet("[controller]/v1/catalogs")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}")]
        [HttpGet("[controller]/v1/portals/{portalId}/catalogs/")]
        public async Task<ActionResult<IEnumerable<iCatalog>>> Catalogs(string orderBy = "CatalogId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, int? portalId = null, int? catalogId = null)
        {
            var query = _context.Catalogs.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (portalId != null) query = query.Where(x => x.PortalId == portalId);
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.CatalogId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.Select(x=>(iCatalog)x).ToListAsync();
        }

        [HttpGet("[controller]/v1/products")]
        [HttpGet("[controller]/v1/products/{productId}")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/products/")]
        public async Task<ActionResult<IEnumerable<iProduct>>> Products(string orderBy = "ProductId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, int? catalogId = null, long? productId = null)
        {
            var query = _context.Products.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (productId != null) query = query.Where(x => x.ProductId == productId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else if (orderBy == "PRICE") query = query.OrderBy(x => x.Price);
            else query = query.OrderBy(x => x.ProductId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.Select(x => (iProduct)x).ToListAsync();
        }


        [HttpGet("[controller]/v1/properties")]     
        [HttpGet("[controller]/v1/propereties/{propertyId}")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/properties/")]  
        public async Task<ActionResult<IEnumerable<iProperty>>> Properties(string orderBy = "PropertyId", string orderMode = "Desc", int page = 0, int pageSize = 50,  int? catalogId = null, int? propertyId =null)
        {
            var query = _context.Properties.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (propertyId != null) query = query.Where(x => x.PropertyId == propertyId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.PropertyId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.Select(x=>(iProperty)x).ToListAsync();
        }       
        
        [HttpGet("[controller]/v1/products/{productId}/characteristics/")]
        [HttpGet("[controller]/v1/characteristics/{characteristicId}")]
        [HttpGet("[controller]/v1/characteristics")]
        public async Task<ActionResult<IEnumerable<iCharacteristic>>> Characteristics(string orderBy = "CharacteristicId", string orderMode = "Desc", int page = 0, int pageSize = 50,  long? productId = null, long? characteristicId =null)
        {
            var query = _context.Characteristics.Include(x=>x.Property).Select(x => x);
            orderBy = orderBy.ToUpper();
            if (characteristicId != null) query = query.Where(x => x.CharacteristicId == characteristicId);
            if (productId != null) query = query.Where(x => x.ProductId == productId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Property.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Property.Title);
            else query = query.OrderBy(x => x.ProductId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.Select(x=>(iCharacteristic)x).ToListAsync();
        }

        [HttpGet("[controller]/v1/catalogs/{catalogId}/images/")]
        [HttpGet("[controller]/v1/catalogs/Images/{imageId}")]
        public async Task<ActionResult<IEnumerable<iCatalogImage>>> CatalogImages(string orderBy = "CatalogImageId", string orderMode = "Desc", int page = 0, int pageSize = 50, int? imageId = null, long? catalogId = null)
        {
            var query = _context.CatalogImages.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (imageId != null) query = query.Where(x => x.Id == imageId);
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.Id);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.Select(x => (iCatalogImage)x).ToListAsync();
        }

        [HttpGet("[controller]/v1/portals/{portalId}/Images/")]
        [HttpGet("[controller]/v1/portals/Images/{imageId}")]
        public async Task<ActionResult<IEnumerable<iPortalImage>>> PortalImages(string orderBy = "PortalImagesId", string orderMode = "Desc", int page = 0, int pageSize = 50, int? imageId=null, int? portalId = null)
        {
            var query = _context.PortalImages.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (portalId != null) query = query.Where(x => x.PortalId == portalId);
            if (imageId != null) query = query.Where(x => x.PortalImageId == imageId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.PortalImageId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.Select(x=>(iPortalImage)x).ToListAsync();
        }

        [HttpGet("[controller]/v1/products/{productId}/images/")]
        [HttpGet("[controller]/v1/products/images/{imageId}")]
        public async Task<ActionResult<IEnumerable<iProductImage>>> ProductImages(string orderBy = "ProductImageId", string orderMode = "Desc", int page = 0, int pageSize = 50, long? imageId = null, long? productId = null)
        {
            var query = _context.ProductImages.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (productId != null) query = query.Where(x => x.ProductId == productId);
            if (imageId != null) query = query.Where(x => x.ProductImageId == imageId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.ProductImageId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.Select(x=>(iProductImage)x).ToListAsync();
        }

        [HttpGet("[controller]/v1/filters")]
        [HttpGet("[controller]/v1/filters/{PropertyId}")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/filters")]
        public async Task<ActionResult<IEnumerable<iFilter>>> Filters(string orderBy = "PropertyId", string orderMode = "Desc", int page = 0, int pageSize = 50, int? catalogId = null, int? propertyId = null)
        {
            var query = _context.Filters.Select(x => x);
            if (catalogId != null) query = query.Where(x=>x.CatalogId == catalogId);
            if (propertyId != null) query = query.Where(x => x.PropertyId ==propertyId);
            orderBy = orderBy.ToUpper();
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else query = query.OrderBy(x => x.PropertyId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);         
            return await query.Select(x=>(iFilter)x).ToListAsync();
        }

        [HttpGet("[controller]/v1/views")]
        [HttpGet("[controller]/v1/portals/{portalId}/views")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/views")]
        [HttpGet("[controller]/v1/users/{userId}/views")]
        public async Task<ActionResult<IEnumerable<Product>>> Products(string orderBy = "ProductId", int page = 0, int pageSize = 50, int? catalogId = null, string userId = null)
        {
            var queryView = _context.Views.Include(x => x.Product).Select(x => x);
            if (userId != null) queryView = queryView.Where(x => x.UserId == userId);
            if (catalogId != null) queryView = queryView.Where(x => x.Product.CatalogId == catalogId);
            queryView.OrderByDescending(x => x.EventDate);
            return await queryView.Select(x => x.Product).Distinct().Skip(page * pageSize).Take(pageSize).ToListAsync();
        }


    }
}

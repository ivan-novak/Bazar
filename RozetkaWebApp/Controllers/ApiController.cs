﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
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
        private readonly RozetkadbContext _context;

        public ApiController(RozetkadbContext context)
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
        [HttpGet("[controller]/v1/promotions/{promotionId}/products/")]
        public async Task<ActionResult<IEnumerable<iProduct>>> Products(string orderBy = "ProductId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, int? catalogId = null, long? productId = null, long? promotionId = null)
        {
            var query = _context.Products.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (productId != null) query = query.Where(x => x.ProductId == productId);
            if (promotionId != null) query = query.Where(x => x.PromotionId == promotionId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else if (orderBy == "PRICE") query = query.OrderBy(x => x.Price);
            else query = query.OrderBy(x => x.ProductId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.Select(x => (iProduct)x).ToListAsync();
        }


        [HttpGet("[controller]/v1/properties")]     
        [HttpGet("[controller]/v1/properties/{propertyId}")]
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

        [HttpGet("[controller]/v1/filters")]
        [HttpGet("[controller]/v1/properties/{propertyId}/filters/")]
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

        [HttpGet("[controller]/v1/orders")]
        [HttpGet("[controller]/v1/orders/{orderId}")]
        [HttpGet("[controller]/v1/users/{userId}/orders")]
        public async Task<ActionResult<IEnumerable<iOrder>>> Orders(string orderMode = "Desc", string orderBy = "OrderId", int page = 0, int pageSize = 50, long? orderId = null, string userId = null)
        {
            orderBy = orderBy.ToUpper();
            var query = _context.Orders.Select(x => x);
            if (userId != null) query = query.Where(x => x.UserId == userId);
            if (orderId != null) query = query.Where(x => x.OrderId == orderId);
            if (orderBy == "STATUS") query = query.OrderByDescending(x => x.Status);
            else if (orderBy == "TOTAL") query = query.OrderByDescending(x => x.Total);
            else if (orderBy == "ORDERDATE") query = query.OrderByDescending(x => x.OrderDate);
            else query = query.OrderByDescending(x => x.OrderId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            return await query.Select(x => (iOrder)x).Distinct().Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        [HttpGet("[controller]/v1/orders/Details")]
        [HttpGet("[controller]/v1/orders/Details/{orderDetailId}")]
        [HttpGet("[controller]/v1/orders/{orderId}/Details")]
        public async Task<ActionResult<IEnumerable<iOrderDetail>>> OrderDetails(string orderMode = "Desc", string orderBy = "OrderDetailId", int page = 0, int pageSize = 50, long? orderId = null, long? orderDetailId = null)
        {
            orderBy = orderBy.ToUpper();
            var query = _context.OrderDetails.Select(x => x);
            if (orderDetailId != null) query = query.Where(x => x.OrderDatailId == orderDetailId);
            if (orderId != null) query = query.Where(x => x.OrderId == orderId);
            if (orderBy == "ORDERID") query = query.OrderByDescending(x => x.OrderId);
            else if (orderBy == "STATUS") query = query.OrderByDescending(x => x.Status);
            else query = query.OrderByDescending(x => x.OrderDatailId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            return await query.Select(x => (iOrderDetail)x).Distinct().Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        [HttpGet("[controller]/v1/addresses")]
        [HttpGet("[controller]/v1/addresses/{addressId}")]
        [HttpGet("[controller]/v1/users/{userId}/addresses")]
        public async Task<ActionResult<IEnumerable<iAddress>>> Addresses(string orderMode = "Desc", string orderBy = "AddressID", int page = 0, int pageSize = 50, string userId = null, long? addressId = null)
        {
            orderBy = orderBy.ToUpper();
            var query = _context.Addresses.Select(x => x);
            if (addressId != null) query = query.Where(x => x.AddressId == addressId);
            if (userId != null) query = query.Where(x => x.UserId == userId);
            if (orderBy == "USERID") query = query.OrderByDescending(x => x.UserId);
            else if (orderBy == "ADDRESSTYPE") query = query.OrderByDescending(x => x.AddressType);
            else query = query.OrderByDescending(x => x.AddressId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            return await query.Select(x => (iAddress)x).Distinct().Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        [HttpGet("[controller]/v1/walletts")]
        [HttpGet("[controller]/v1/walletts/{walettesId}")]
        [HttpGet("[controller]/v1/users/{userId}/wallettes")]
        public async Task<ActionResult<IEnumerable<iWallett>>> Wallettes(string orderMode = "Desc", string orderBy = "WalletteId", int page = 0, int pageSize = 50, string userId = null, long? walletteId = null)
        {
            orderBy = orderBy.ToUpper();
            var query = _context.Walletts.Select(x => x);
            if (walletteId != null) query = query.Where(x => x.WalletId == walletteId);
            if (userId != null) query = query.Where(x => x.UserId == userId);
            if (orderBy == "CARDTYPE") query = query.OrderByDescending(x => x.CardType);
            else if (orderBy == "USERID") query = query.OrderByDescending(x => x.UserId);
            else query = query.OrderByDescending(x => x.WalletId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            return await query.Select(x => (iWallett)x).Distinct().Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        [HttpGet("[controller]/v1/contacts")]
        [HttpGet("[controller]/v1/contacts/{contactId}")]
        [HttpGet("[controller]/v1/users/{userId}/contacts")]
        public async Task<ActionResult<IEnumerable<iContact>>> Contacts(string orderMode = "Desc", string orderBy = "WalletteId", int page = 0, int pageSize = 50, string userId = null, long? contactId = null)
        {
            orderBy = orderBy.ToUpper();
            var query = _context.Contacts.Select(x => x);
            if (contactId != null) query = query.Where(x => x.ContactId == contactId);
            if (userId != null) query = query.Where(x => x.UserId == userId);
            if (orderBy == "EMAIL") query = query.OrderByDescending(x => x.Email);
            else if (orderBy == "USERID") query = query.OrderByDescending(x => x.UserId);
            else query = query.OrderByDescending(x => x.ContactId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            return await query.Select(x => (iContact)x).Distinct().Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        [HttpGet("[controller]/v1/comments")]
        [HttpGet("[controller]/v1/comments/{commentId}")]
        [HttpGet("[controller]/v1/users/{userId}/comments")]
        [HttpGet("[controller]/v1/products/{productId}/comments")]
        public async Task<ActionResult<IEnumerable<iComment>>> Comments(string orderMode = "Desc", string orderBy = "CommentId", int page = 0, int pageSize = 50, string userId = null, long? commentId = null, long? productId = null)
        {
            orderBy = orderBy.ToUpper();
            var query = _context.Comments.Select(x => x);
            if (commentId != null) query = query.Where(x => x.CommentId == commentId);
            if (productId != null) query = query.Where(x => x.ProductId == productId);
            if (userId != null) query = query.Where(x => x.UserId == userId);
            if (orderBy == "PRODUCTID") query = query.OrderByDescending(x => x.ProductId);
            else if (orderBy == "SCORE") query = query.OrderByDescending(x => x.Score);
            else if (orderBy == "USERID") query = query.OrderByDescending(x => x.UserId);
            else query = query.OrderByDescending(x => x.CommentId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            return await query.Select(x => (iComment)x).Distinct().Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        [HttpGet("[controller]/v1/promotions")]
        [HttpGet("[controller]/v1/promotions/{promotionsId}")]
        public async Task<ActionResult<IEnumerable<iPromotion>>> Promotions(string orderMode = "Desc", string orderBy = "PromotionId", int page = 0, int pageSize = 50, string userId = null, long? promotionId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            orderBy = orderBy.ToUpper();
            var query = _context.Promotions.Select(x => x);
            if (promotionId != null) query = query.Where(x => x.PromotionId == promotionId);
            if (startDate != null) query = query.Where(x => x.StartDate >= startDate);
            if (endDate != null) query = query.Where(x => x.EndDate >= endDate);
            else if (orderBy == "PROMOTIONID") query = query.OrderByDescending(x => x.PromotionId);
            else if (orderBy == "STARTDATE") query = query.OrderByDescending(x => x.StartDate);
            else query = query.OrderByDescending(x => x.PromotionId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            return await query.Select(x => (iPromotion)x).Distinct().Skip(page * pageSize).Take(pageSize).ToListAsync();
        }
       
        [HttpGet("[controller]/v1/products/{productId}/images")]
        [HttpGet("[controller]/v1/products/{productId}/images/{label}")]
        [HttpGet("[controller]/v1/products/images")]
        [HttpGet("[controller]/v1/products/images/{productImageId}")]
        public async Task<ActionResult<IEnumerable<iProductImage>>> ProductImages(string orderBy = "ProductImageId", string orderMode = "Desc", int page = 0, int pageSize = 50, string label = null, long? productId = null, long? productImageId = null)
        {
            var query = _context.ProductImages.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (productImageId != null) query = query.Where(x => x.ProductImageId == productImageId);
            if (productId != null) query = query.Where(x => x.ProductId == productId);
            if (label != null) query = query.Where(x => x.Label ==label);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.ProductImageId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.Select(x => (iProductImage)x).ToListAsync();
        }

        [HttpGet("[controller]/v1/portals/{portalId}/images")]
        [HttpGet("[controller]/v1/portals/{portalId}/images/{label}")]
        [HttpGet("[controller]/v1/portals/images")]
        [HttpGet("[controller]/v1/portals/images/{portalImageId}")]
        public async Task<ActionResult<IEnumerable<iPortalImage>>> PortalImages(string orderBy = "PortalImagesId", string orderMode = "Desc", int page = 0, int pageSize = 50, string label = null, int? portalId = null, int? portalImageId = null)
        {
            var query = _context.PortalImages.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (portalImageId != null) query = query.Where(x => x.PortalImageId == portalImageId);
            if (portalId != null) query = query.Where(x => x.PortalId == portalId);
            if (label != null) query = query.Where(x => x.Label == label);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.PortalImageId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.Select(x => (iPortalImage)x).ToListAsync();
        }

        [HttpGet("[controller]/v1/catalogs/{catalogId}/images")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/images/{label}")]
        [HttpGet("[controller]/v1/catalogs/Images")]
        [HttpGet("[controller]/v1/catalogs/Images/{catalogimageId}")]
        public async Task<ActionResult<IEnumerable<iCatalogImage>>> CatalogImages(string orderBy = "CatalogImageId", string orderMode = "Desc", int page = 0, int pageSize = 50, string label = null, int? catalogImageId = null, long? catalogId = null)
        {
            var query = _context.CatalogImages.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (catalogImageId != null) query = query.Where(x => x.Id == catalogImageId);
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (label != null) query = query.Where(x => x.Label == label);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.Id);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            return await query.Select(x => (iCatalogImage)x).ToListAsync();
        }

        [HttpGet("[controller]/v1/views")]
        [HttpGet("[controller]/v1/portals/{portalId}/views")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/views")]
        [HttpGet("[controller]/v1/users/{userId}/views")]
        public async Task<ActionResult<IEnumerable<iProduct>>> Products(string orderBy = "ProductId", int page = 0, int pageSize = 50, int? catalogId = null, string userId = null)
        {
            // var userId1 = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sesionId = CartId();

            var queryView = _context.Views.Include(x => x.Product).Select(x => x);
            if (userId != null) queryView = queryView.Where(x => x.UserId == userId);
            if (catalogId != null) queryView = queryView.Where(x => x.Product.CatalogId == catalogId);
            queryView.OrderByDescending(x => x.EventDate);
            return await queryView.Select(x => (iProduct)x.Product).Distinct().Skip(page * pageSize).Take(pageSize).ToListAsync();
        }


        public string CartId()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("cartId"))
            {
                var cartId = Guid.NewGuid().ToString();
                HttpContext.Response.Cookies.Append("cartId", cartId);
                return cartId;
            }
            return HttpContext.Request.Cookies["cartId"];
        }
    }
}

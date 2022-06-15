//MLHIDEFILE
using System;
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
        public async Task<ApiResult<iPortal>> Portals(string orderBy = "PortalId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, int? portalId = null)
        {
            var query = _context.Portals.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (portalId != null) query = query.Where(x => x.PortalId == portalId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.PortalId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iPortal> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/catalogs")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}")]
        [HttpGet("[controller]/v1/portals/{portalId}/catalogs/")]
        public async Task<ApiResult<iCatalog>> Catalogs(string orderBy = "CatalogId", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, int? portalId = null, int? catalogId = null)
        {
            var query = _context.Catalogs.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (portalId != null) query = query.Where(x => x.PortalId == portalId);
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.CatalogId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iCatalog>{ TotalCount = totalCount, Values = values };
        }

       [HttpPut("[controller]/v1/catalogs/{catalogId}/products/")]
       [HttpPost("[controller]/v1/catalogs/{catalogId}/products/")]
        public async Task<ApiResult<iProduct>> Products(string orderBy = "ProductId", string orderMode = "Desc", int page = 0, int pageSize = 50,
                 int? catalogId = null,  long? promotionId = null, [FromBody] Filter[] filters = null)
        {
            IQueryable<Product> query;
            if (filters != null)
            {
                var query1 = _context.Characteristics.Select(x => x);
                foreach(var i in filters) if (i.Value != null) query1 = query1.Where(x => x.PropertyId == i.PropertyId && i.Value.Contains(x.Value));
                query = query1.Include(x => x.Product).Where(x => x.Product.CatalogId == catalogId).Select(x => x.Product).Distinct();
            }
            else
            {
                query = _context.Products.Select(x => x);
            }
            orderBy = orderBy.ToUpper();
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (promotionId != null) query = query.Where(x => x.PromotionId == promotionId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else if (orderBy == "PRICE") query = query.OrderBy(x => x.Price);
            else query = query.OrderBy(x => x.ProductId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iProduct> { TotalCount = totalCount, Values = values };
        }


        [HttpGet("[controller]/v1/products")]
        [HttpGet("[controller]/v1/products/{productId}")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/products/")]
        [HttpGet("[controller]/v1/promotions/{promotionId}/products/")]
        public async Task<ApiResult<iProduct>> Products(string orderBy = "ProductId", string orderMode = "Desc", int page = 0, int pageSize = 50, 
            int? catalogId = null, long? productId = null, long? promotionId = null)
        {
            if(productId!= null)
            {
                var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartId = CartId();
                IQueryable<View> queryView = null;
                if (user_Id != null) queryView = _context.Views.Where(x => x.UserId == user_Id);
                else queryView = _context.Views.Where(x => x.CartId == cartId);
                View lineView = queryView.Where(x => x.ProductId == productId).FirstOrDefault();
                if (lineView == null)
                {
                    lineView = new View();
                    lineView.CartId = CartId();
                    lineView.UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    lineView.ProductId = productId;
                    lineView.EventDate = DateTime.Now;
                    _context.Add(lineView);
                }
                else
                {
                    lineView.EventDate = DateTime.Now;
                    _context.Update(lineView);
                }
                await _context.SaveChangesAsync();
            }
            var  query = _context.Products.Select(x => x);            
            orderBy = orderBy.ToUpper();
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (productId != null) query = query.Where(x => x.ProductId == productId);
            if (promotionId != null) query = query.Where(x => x.PromotionId == promotionId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else if (orderBy == "PRICE") query = query.OrderBy(x => x.Price);
            else query = query.OrderBy(x => x.ProductId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iProduct> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/properties")]     
        [HttpGet("[controller]/v1/properties/{propertyId}")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/properties/")]  
        public async Task<ApiResult<iProperty>> Properties(string orderBy = "PropertyId", string orderMode = "Desc", int page = 0, int pageSize = 50,  int? catalogId = null, int? propertyId =null)
        {
            var query = _context.Properties.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (catalogId != null) query = query.Where(x => x.CatalogId == catalogId);
            if (propertyId != null) query = query.Where(x => x.PropertyId == propertyId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Title);
            else query = query.OrderBy(x => x.PropertyId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iProperty> { TotalCount = totalCount, Values = values };
        }       
        
        [HttpGet("[controller]/v1/products/{productId}/characteristics/")]
        [HttpGet("[controller]/v1/characteristics/{characteristicId}")]
        [HttpGet("[controller]/v1/characteristics")]
        public async Task<ApiResult<iCharacteristic>> Characteristics(string orderBy = "CharacteristicId", string orderMode = "Desc", int page = 0, int pageSize = 50,  long? productId = null, long? characteristicId =null)
        {
            var query = _context.Characteristics.Include(x=>x.Property).Select(x => x);
            orderBy = orderBy.ToUpper();
            if (characteristicId != null) query = query.Where(x => x.CharacteristicId == characteristicId);
            if (productId != null) query = query.Where(x => x.ProductId == productId);
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Property.Label);
            else if (orderBy == "TITLE") query = query.OrderBy(x => x.Property.Title);
            else query = query.OrderBy(x => x.ProductId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iCharacteristic> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/filters")]
        [HttpGet("[controller]/v1/properties/{propertyId}/filters/")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/filters")]
        public async Task<ApiResult<iFilter>> Filters(string orderBy = "PropertyId", string orderMode = "Desc", int page = 0, int pageSize = 50, int? catalogId = null, int? propertyId = null)
        {
            var query = _context.Filters.Select(x => x);
            if (catalogId != null) query = query.Where(x=>x.CatalogId == catalogId);
            if (propertyId != null) query = query.Where(x => x.PropertyId ==propertyId);
            orderBy = orderBy.ToUpper();
            if (orderBy == "LABEL") query = query.OrderBy(x => x.Label);
            else query = query.OrderBy(x => x.PropertyId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iFilter> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/orders")]
        [HttpGet("[controller]/v1/orders/{orderId}")]
        [HttpGet("[controller]/v1/users/{userId}/orders")]
        public async Task<ApiResult<iOrder>> Orders(string orderMode = "Desc", string orderBy = "OrderId", int page = 0, int pageSize = 50, long? orderId = null, string userId = null)
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
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iOrder> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/orders/Details")]
        [HttpGet("[controller]/v1/orders/Details/{orderDetailId}")]
        [HttpGet("[controller]/v1/orders/{orderId}/Details")]
        public async Task<ApiResult<iLineDetail>> OrderDetails(string orderMode = "Desc", string orderBy = "OrderDetailId", int page = 0, int pageSize = 50, long? orderId = null, long? orderDetailId = null)
        {
            orderBy = orderBy.ToUpper();
            var query = _context.LineDetails.Select(x => x).Where(x=>x.OrderId != null);
            if (orderDetailId != null) query = query.Where(x => x.OrderDatailId == orderDetailId);
            if (orderId != null) query = query.Where(x => x.OrderId == orderId);
            if (orderBy == "ORDERID") query = query.OrderByDescending(x => x.OrderId);
            else if (orderBy == "STATUS") query = query.OrderByDescending(x => x.Status);
            else query = query.OrderByDescending(x => x.OrderDatailId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iLineDetail> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/addresses")]
        [HttpGet("[controller]/v1/addresses/{addressId}")]
        [HttpGet("[controller]/v1/users/{userId}/addresses")]
        public async Task<ApiResult<iAddress>> Addresses(string orderMode = "Desc", string orderBy = "AddressID", int page = 0, int pageSize = 50, string userId = null, long? addressId = null)
        {
            orderBy = orderBy.ToUpper();
            var query = _context.Addresses.Select(x => x);
            if (addressId != null) query = query.Where(x => x.AddressId == addressId);
            if (userId != null) query = query.Where(x => x.UserId == userId);
            if (orderBy == "USERID") query = query.OrderByDescending(x => x.UserId);
            else if (orderBy == "ADDRESSTYPE") query = query.OrderByDescending(x => x.AddressType);
            else query = query.OrderByDescending(x => x.AddressId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iAddress> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/walletts")]
        [HttpGet("[controller]/v1/walletts/{walletteId}")]
        [HttpGet("[controller]/v1/users/{userId}/wallettes")]
        public async Task<ApiResult<iWallett>> Wallettes(string orderMode = "Desc", string orderBy = "WalletteId", int page = 0, int pageSize = 50, string userId = null, long? walletteId = null)
        {
            orderBy = orderBy.ToUpper();
            var query = _context.Walletts.Select(x => x);
            if (walletteId != null) query = query.Where(x => x.WalletId == walletteId);
            if (userId != null) query = query.Where(x => x.UserId == userId);
            if (orderBy == "CARDTYPE") query = query.OrderByDescending(x => x.CardType);
            else if (orderBy == "USERID") query = query.OrderByDescending(x => x.UserId);
            else query = query.OrderByDescending(x => x.WalletId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iWallett> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/contacts")]
        [HttpGet("[controller]/v1/contacts/{contactId}")]
        [HttpGet("[controller]/v1/users/{userId}/contacts")]
        public async Task<ApiResult<iContact>> Contacts(string orderMode = "Desc", string orderBy = "WalletteId", int page = 0, int pageSize = 50, string userId = null, long? contactId = null)
        {
            orderBy = orderBy.ToUpper();
            var query = _context.Contacts.Select(x => x);
            if (contactId != null) query = query.Where(x => x.ContactId == contactId);
            if (userId != null) query = query.Where(x => x.UserId == userId);
            if (orderBy == "EMAIL") query = query.OrderByDescending(x => x.Email);
            else if (orderBy == "USERID") query = query.OrderByDescending(x => x.UserId);
            else query = query.OrderByDescending(x => x.ContactId);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iContact> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/comments")]
        [HttpGet("[controller]/v1/comments/{commentId}")]
        [HttpGet("[controller]/v1/users/{userId}/comments")]
        [HttpGet("[controller]/v1/products/{productId}/comments")]
        public async Task<ApiResult<iComment>> Comments(string orderMode = "Desc", string orderBy = "CommentId", int page = 0, int pageSize = 50, string userId = null, long? commentId = null, long? productId = null)
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
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iComment> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/promotions")]
        [HttpGet("[controller]/v1/promotions/{promotionsId}")]
        public async Task<ApiResult<iPromotion>> Promotions(string orderMode = "Desc", string orderBy = "PromotionId", int page = 0, int pageSize = 50, string userId = null, long? promotionId = null, DateTime? startDate = null, DateTime? endDate = null)
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
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iPromotion> { TotalCount = totalCount, Values = values };
        }
       
        [HttpGet("[controller]/v1/products/{productId}/images")]
        [HttpGet("[controller]/v1/products/{productId}/images/{label}")]
        [HttpGet("[controller]/v1/products/images")]
        [HttpGet("[controller]/v1/products/images/{productImageId}")]
        public async Task<ApiResult<iProductImage>> ProductImages(string orderBy = "ProductImageId", string orderMode = "Desc", int page = 0, int pageSize = 50, string label = null, long? productId = null, long? productImageId = null)
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
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iProductImage> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/portals/{portalId}/images")]
        [HttpGet("[controller]/v1/portals/{portalId}/images/{label}")]
        [HttpGet("[controller]/v1/portals/images")]
        [HttpGet("[controller]/v1/portals/images/{portalImageId}")]
        public async Task<ApiResult<iPortalImage>> PortalImages(string orderBy = "PortalImagesId", string orderMode = "Desc", int page = 0, int pageSize = 50, string label = null, int? portalId = null, int? portalImageId = null)
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
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iPortalImage> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/catalogs/{catalogId}/images")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/images/{label}")]
        [HttpGet("[controller]/v1/catalogs/Images")]
        [HttpGet("[controller]/v1/catalogs/Images/{catalogimageId}")]
        public async Task<ApiResult<iCatalogImage>> CatalogImages(string orderBy = "CatalogImageId", string orderMode = "Desc", int page = 0, int pageSize = 50, string label = null, int? catalogImageId = null, long? catalogId = null)
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
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iCatalogImage> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/views")]
        [HttpGet("[controller]/v1/portals/{portalId}/views")]
        [HttpGet("[controller]/v1/catalogs/{catalogId}/views")]
        [HttpGet("[controller]/v1/users/{userId}/views")]
        public async Task<ApiResult<iProduct>> Products(string orderBy = "ProductId", int page = 0, int pageSize = 50, int? catalogId = null, string userId = null)
        {
            var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sesionId = CartId();
            var query = _context.Views.Include(x => x.Product).Select(x => x);
            if (userId != null) query = query.Where(x => x.UserId == userId);
            if (catalogId != null) query = query.Where(x => x.Product.CatalogId == catalogId);
            query.OrderByDescending(x => x.EventDate);
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x.Product).ToListAsync();
            return new ApiResult<iProduct> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("[controller]/v1/cart")]
        public async Task<ApiResult<iLineDetail>> LineDetail(int page = 0, int pageSize = 50)
        {
            var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartId = CartId();
            var query = _context.LineDetails.Include(x => x.Product).Where(x => x.OrderId == null);
            if (user_Id != null) query = query.Where(x => x.UserId == user_Id);
            else query = query.Where(x => x.CartId == cartId);
            query.OrderByDescending(x => x.CreateDate);
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iLineDetail> { TotalCount = totalCount, Values = values };
        }

        // GET: Portals
        [HttpGet("[controller]/v1/users")]
        [HttpGet("[controller]/v1/users/{Id}")]
        public async Task<ApiResult<iAspNetUser>> Users(string orderBy = "Id", string orderMode = "Desc", int page = 0, int pageSize = 50, string searchTerm = null, string Id = null)
        {
            var query = _context.AspNetUsers.Select(x => x);
            orderBy = orderBy.ToUpper();
            if (Id != null) query = query.Where(x => x.Id == Id);
            if (orderBy == "Email") query = query.OrderBy(x => x.Email);
            else if (orderBy == "UserName") query = query.OrderBy(x => x.UserName);
            else query = query.OrderBy(x => x.Id);
            if (orderMode.ToUpper() == "ASC") query = query.Reverse();
            query = query.Skip(page * pageSize).Take(pageSize);
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iAspNetUser> { TotalCount = totalCount, Values = values };
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

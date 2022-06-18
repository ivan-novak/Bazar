//MLHIDEFILE
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;

namespace RozetkaWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly RozetkadbContext _context;
        private UserManager<IdentityUser> _userManager;
        public readonly SignInManager<IdentityUser> _signInManager;


        public AccountController(RozetkadbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }


        [HttpPost("api/v1/account/address/")]
        public async Task<ActionResult<iAddress>> PostAddress([FromBody] Address address)
        {
            var newItem = new Address
            {
                AddressType = address.AddressType,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country
            };
            _context.Addresses.Add(newItem);
            await _context.SaveChangesAsync();
            return newItem;
        }


        [HttpPut("api/v1/account/Address/")]
        public async Task<ActionResult> PutAddress([FromBody] Address address)
        {
            var Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.Addresses.FindAsync(address.AddressId);
            if (newItem == null || newItem.UserId != Id)   return NotFound();  
            newItem.AddressType = address.AddressType?.ToString();
            newItem.AddressLine1 = address.AddressLine1?.ToString();
            newItem.AddressLine2 = address.AddressLine2?.ToString();
            newItem.AddressLine3 = address.AddressLine3?.ToString();  
            newItem.UserId = Id;
            newItem.City = address.City?.ToString();      
            newItem.State = address.State?.ToString();        
            newItem.PostalCode = address.PostalCode?.ToString();       
            newItem.Country = address.Country?.ToString();
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("api/v1/account/Address/{Id}")]
        public async Task<IActionResult> DeleteAddress(long Id)
        {
            var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.Addresses.FindAsync(Id);
            if (newItem == null || newItem.UserId != user_Id) return NotFound();
            _context.Addresses.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("api/v1/account/Address/")]
        [HttpGet("api/v1/account/Address/{Id}")]
        public async Task<ApiResult<iAddress>> Addresses(string orderMode = "ASC", string orderBy = "addressId", int page = 0, int pageSize = 50, long? Id = null)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            orderBy = orderBy.ToUpper();
            var query = _context.Addresses.Select(x => x);
            if (Id != null) query = query.Where(x => x.AddressId == Id);
            query = query.Where(x => x.UserId == userId);
            if (orderBy == "USERID") query = query.OrderBy(x => x.UserId);
            else if (orderBy == "ADDRESSTYPE") query = query.OrderBy(x => x.AddressType);
            else query = query.OrderBy(x => x.AddressId);
            if (orderMode.ToUpper() == "DESC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iAddress> { TotalCount = totalCount, Values = values };
        }


        [HttpPost("api/v1/users/{Id}/Contact/")]
        public async Task<ActionResult<iContact>> PostContact([FromBody] Contact contact)
        {
            var newItem = new Contact
            {
                ContactType = contact.ContactType,
                DisplayName = contact.DisplayName,
                FullName = contact.FullName,
                Title = contact.Title,
                Salutation = contact.Salutation,
                UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                Attention = contact.Attention,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                WebSite = contact.WebSite,
                Fax = contact.Fax,
                FaxType = contact.FaxType,
                Phone1 = contact.Phone1,
                Phone1Type = contact.Phone1Type,
                Phone2 = contact.Phone2,
                Phone2Type = contact.Phone2Type,
                Phone3 = contact.Phone3,
                Phone3Type = contact.Phone3Type,
                DefAddressId = contact.DefAddressId,
                AssignDate = contact.AssignDate,
                ExtAddressId = contact.ExtAddressId
            };
            _context.Contacts.Add(newItem);
            await _context.SaveChangesAsync();
            return newItem;
        }


        [HttpPut("api/v1/account/Contact/")]
        public async Task<ActionResult> PutContact([FromBody] Contact contact)
        {
            var Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.Contacts.FindAsync(contact.ContactId);
            if (newItem == null || newItem.UserId != Id)  return NotFound();
            newItem.ContactType = contact.ContactType?.ToString(); 
            newItem.DisplayName = contact.DisplayName?.ToString();
            newItem.FullName = contact.FullName?.ToString();
            newItem.Title = contact.Title?.ToString();
            newItem.Salutation = contact.Salutation?.ToString();
            newItem.UserId = Id?.ToString();
            newItem.Attention = contact.Attention?.ToString();
            newItem.FirstName = contact.FirstName?.ToString();
            newItem.LastName = contact.LastName?.ToString();
            newItem.Email = contact.Email?.ToString();
            newItem.WebSite = contact.WebSite?.ToString();
            newItem.Fax = contact.Fax?.ToString();
            newItem.FaxType = contact.FaxType?.ToString();
            newItem.Phone1 = contact.Phone1?.ToString();
            newItem.Phone1Type = contact.Phone1Type;
            newItem.Phone2 = contact.Phone2?.ToString();
            newItem.Phone2Type = contact.Phone2Type?.ToString();
            newItem.Phone3 = contact.Phone3?.ToString();
            newItem.Phone3Type = contact.Phone3Type?.ToString();
            newItem.DefAddressId = contact.DefAddressId;
            newItem.AssignDate = contact.AssignDate;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("api/v1/account/Contact/")]
        public async Task<IActionResult> DeleteContact(long Id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.Contacts.FindAsync(Id);
            if (newItem == null || newItem.UserId != userId) return NotFound();
            _context.Contacts.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("api/v1/account/contacts")]
        [HttpGet("api/v1/account/contacts/{Id}")]
        public async Task<ApiResult<iContact>> Contacts(string orderMode = "ASC", string orderBy = "ContactId", int page = 0, int pageSize = 50,  long? Id = null)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            orderBy = orderBy.ToUpper();
            var query = _context.Contacts.Select(x => x);
            if (Id != null) query = query.Where(x => x.ContactId == Id);
            query = query.Where(x => x.UserId == userId);
            if (orderBy == "EMAIL") query = query.OrderBy(x => x.Email);
            else if (orderBy == "USERID") query = query.OrderBy(x => x.UserId);
            else query = query.OrderBy(x => x.ContactId);
            if (orderMode.ToUpper() == "DESC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iContact> { TotalCount = totalCount, Values = values };
        }


        [HttpPost("api/v1/account/wallets/")]
        public async Task<ActionResult<iWallett>> PostWallett([FromBody] Wallett wallet)
        {
            var Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = new Wallett
            {
                CardType = wallet.CardType,
                ExpiryDate = wallet.ExpiryDate,
                Cardholder = wallet.Cardholder,
                CardNumber = wallet.CardNumber,
                UserId = Id
            };
            _context.Walletts.Add(newItem);
            await _context.SaveChangesAsync();
            return newItem;
        }


        [HttpPut("api/v1/account/wallets/")]
        public async Task<ActionResult> PutWallett([FromBody] Wallett Wallett)
        {
            var Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.Walletts.FindAsync(Wallett.WalletId);
            if (newItem == null || newItem.UserId != Id) return NotFound();
            newItem.CardType = Wallett.CardType?.ToString();
            newItem.ExpiryDate = Wallett.ExpiryDate?.ToString();
            newItem.Cardholder = Wallett.Cardholder?.ToString();
            newItem.CardNumber = Wallett.CardNumber?.ToString();
            newItem.UserId ??= Id?.ToString();
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("api/v1/account/wallets/{Id}")]
        public async Task<IActionResult> DeleteWallett(long Id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.Walletts.FindAsync(Id);
            if (newItem == null || newItem.UserId != userId) return NotFound();
            _context.Walletts.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("api/v1/account/wallets/")]
        [HttpGet("api/v1/account/wallets/{Id}")]
        public async Task<ApiResult<iWallett>> Wallettes(string orderMode = "ASC", string orderBy = "WalletteId", int page = 0, int pageSize = 50,  long? Id = null)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            orderBy = orderBy.ToUpper();
            var query = _context.Walletts.Select(x => x);
            if (Id != null) query = query.Where(x => x.WalletId == Id);
            query = query.Where(x => x.UserId == userId);
            if (orderBy == "CARDTYPE") query = query.OrderBy(x => x.CardType);
            else if (orderBy == "USERID") query = query.OrderBy(x => x.UserId);
            else query = query.OrderBy(x => x.WalletId);
            if (orderMode.ToUpper() == "DESC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iWallett> { TotalCount = totalCount, Values = values };
        }

        [HttpPost("api/v1/account/comments/")]
        public async Task<ActionResult<iComment>> PostComment( [FromBody] Comment Comment)
        {
            var newItem = new Comment
            {
                ProductId = Comment.ProductId,
                Text = Comment.Text,
                Date = Comment.Date,
                Score = Comment.Score,
                ImageId = Comment.ImageId,
                UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
        };
            _context.Comments.Add(newItem);
            await _context.SaveChangesAsync();
            return newItem;
        }


        [HttpPut("api/v1/account/comments/")]
        public async Task<ActionResult> PutComment([FromBody] Comment Comment)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.Comments.FindAsync(Comment.CommentId);
            if (newItem == null || newItem.UserId != userId)  return NotFound();
            newItem.ProductId = Comment.ProductId;
            newItem.Text = Comment.Text?.ToString();
            newItem.Date = Comment.Date;
            newItem.Score = Comment.Score;
            newItem.ImageId = Comment.ImageId;
            newItem.UserId = userId;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("api/v1/account/comments/{Id}")]
        public async Task<IActionResult> DeleteComment(long Id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.Comments.FindAsync(Id);
            if (newItem == null || newItem.UserId != userId) return NotFound();
            _context.Comments.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("api/v1/account/comments/")]
        [HttpGet("api/v1/account/comments/{Id}")]
        public async Task<ApiResult<iComment>> Comments(string orderMode = "ASC", string orderBy = "CommentId", int page = 0, int pageSize = 50, long? Id = null, long? productId = null)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            orderBy = orderBy.ToUpper();
            var query = _context.Comments.Select(x => x);
            if (Id != null) query = query.Where(x => x.CommentId == Id);
            if (productId != null) query = query.Where(x => x.ProductId == productId);
            query = query.Where(x => x.UserId == userId);
            if (orderBy == "PRODUCTID") query = query.OrderBy(x => x.ProductId);
            else if (orderBy == "SCORE") query = query.OrderBy(x => x.Score);
            else if (orderBy == "USERID") query = query.OrderBy(x => x.UserId);
            else query = query.OrderBy(x => x.CommentId);
            if (orderMode.ToUpper() == "DESC") query = query.Reverse();
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iComment> { TotalCount = totalCount, Values = values };
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

        public class UserModel
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public bool? EmailConfirmed { get; set; }
            public string Password { get; set; }
            public string PhoneNumber { get; set; }
            public bool? PhoneNumberConfirmed { get; set; }
            public bool? TwoFactorEnabled { get; set; }
           public bool? RememberMe { get; set; }
        }
   

        [HttpPost("api/v1/account/signon")]
        public async Task<ActionResult<UserModel>> PostAspNetUser([FromBody] UserModel Input)
        {
            var user = new IdentityUser { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            }
            Input.Id = user.Id;
            return Input;
        }

        [HttpPut("api/v1/account/update/")]
        public async Task<ActionResult<UserModel>> PutAspNetUser([FromBody] UserModel Input)
        {
            var Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.AspNetUsers.FindAsync(Id);
            if (newItem == null) return NotFound();
            newItem.UserName = Input.UserName?.ToString();
            newItem.PhoneNumber = Input.PhoneNumber?.ToString();
            if (Input.TwoFactorEnabled != null) newItem.TwoFactorEnabled = (bool)Input.TwoFactorEnabled;
            if (Input.EmailConfirmed != null) newItem.TwoFactorEnabled = (bool)Input.EmailConfirmed;
            await _context.SaveChangesAsync();
            if (Input.Password != null) 
            {
                var user = await _userManager.GetUserAsync(User);
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, Input.Password);
                await _signInManager.RefreshSignInAsync(user);
            }
            Input.Id = Id;
            return Input;
        }

        [HttpDelete("api/v1/account/logout/")]
        public async Task<IActionResult> DeleteAspNetLogOut()
        {
            var Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.AspNetUsers.FindAsync(Id);
            if (newItem == null || newItem.Id != Id) return NotFound();
            return NoContent();
        }

        [HttpDelete("api/v1/account/signoff/")]
        public async Task<IActionResult> DeleteAspNetUser()
        {
            var Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.AspNetUsers.FindAsync(Id);
            if (newItem == null || newItem.Id != Id)return NotFound();
            _context.AspNetUsers.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("api/v1/account/Login/")]
        public async Task<IActionResult> Login([FromBody] UserModel user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.Email,
            user.Password, (bool)user.RememberMe, lockoutOnFailure: true);
            return NoContent();
        }

        [HttpGet("api/v1/account/cart/details")]
        [HttpGet("api/v1/account/cart/details/{Id}")]
        public async Task<ApiResult<iLineDetail>> CartDetail(long? Id = null, int page = 0, int pageSize = 50)
        {
            var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartId = CartId();
            var query = _context.LineDetails.Include(x => x.Product).Where(x => x.OrderId == null);
            if (user_Id != null) query = query.Where(x => x.UserId == user_Id);
            else query = query.Where(x => x.CartId == cartId);
            if (Id != null) query = query.Where(x => x.OrderDatailId == Id);
            query.OrderByDescending(x => x.CreateDate);
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iLineDetail> { TotalCount = totalCount, Values = values };
        }


        [HttpPost("api/v1/account/cart/details/")]
        public async Task<ActionResult<iLineDetail>> PostLineDetail([FromBody] LineDetail LineDetail)
        {
            var Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = new LineDetail
            {
                ProductId = LineDetail.ProductId,
                CartId = CartId(),
                Quantities = LineDetail.Quantities,
                UnitCost = _context.Products.Find(LineDetail.ProductId).Price,
                CreateDate = DateTime.Now,
                UserId = Id
            };
            _context.LineDetails.Add(newItem);
            await _context.SaveChangesAsync();
            return newItem;
        }


        [HttpPut("api/v1/account/cart/details/{Id}")]
        public async Task<ActionResult> PutLineDetail(string Id, [FromBody] LineDetail LineDetail)
        {
            var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.LineDetails.FindAsync(LineDetail.OrderDatailId);
            if (newItem == null || newItem.UserId != user_Id) return NotFound();
            newItem.ProductId = LineDetail.ProductId;
            newItem.Quantities = LineDetail.Quantities;
            newItem.UnitCost = _context.Products.Find(LineDetail.ProductId).Price;
            newItem.UserId = user_Id;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("api/v1/account/cart/details/{Id}")]
        public async Task<IActionResult> DeleteLineDetail(string Id, LineDetail LineDetail)
        {
            var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newItem = await _context.LineDetails.FindAsync(LineDetail.OrderDatailId);
            if (newItem == null || newItem.UserId != user_Id) return NotFound();
            _context.LineDetails.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPost("api/v1/account/order/")]
        public async Task<ActionResult<iOrder>> PostOrder([FromBody] Order Order)
        {
            var Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Id == null) return NotFound();
            var newItem = new Order
            {
                Description = Order.Description,
                Total = Order.Total,
                OrderDate = DateTime.Now,
                CardNumber = Order.CardNumber,
                DeliveryAddress = Order.DeliveryAddress,
                DeliveryContact = Order.DeliveryContact,
                DeliveryEmail = Order.DeliveryEmail,
                DeliveryPhone = Order.DeliveryPhone,
                UserId = Id
            };
            var cart = _context.LineDetails.Where(a => (a.CartId == CartId() || a.UserId == Id) && a.OrderId == null).Include(a => a.Product).ToList();
            if (cart.Count ==0) return NotFound();
            if (Order.Description == null) newItem.Description = String.Join(",", cart.Select(s => s.Product.Label));
            newItem.Total = cart.Sum(x => x.LineTotal);
            _context.Orders.Add(newItem);
            await _context.SaveChangesAsync();
            foreach (var i in cart)
            {
                i.OrderId = newItem.OrderId;
                _context.Update(i);
            }
            await _context.SaveChangesAsync();
            return newItem;
        }


        [HttpGet("api/v1/account/orders/")]
        [HttpGet("api/v1/account/orders/{Id}")]
        public async Task<ApiResult<iOrder>> Orders(long? Id=null, int page = 0, int pageSize = 50)
        {
            var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = _context.Orders.Where(x => x.UserId == user_Id);
            if (Id != null) query = query.Where(x => x.OrderId == Id);
            query.OrderByDescending(x => x.OrderDate);
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iOrder> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("api/v1/account/orders/{Id}/details")]
        public async Task<ApiResult<iLineDetail>> OrderDetail(long? Id=null, int page = 0, int pageSize = 50)
        {
            var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = _context.LineDetails.Include(x => x.Product).Where(x => x.OrderId != null && x.UserId == user_Id);
            if (Id != null) query = query.Where(x => x.OrderId == Id);
            query.OrderByDescending(x => x.CreateDate);
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x).ToListAsync();
            return new ApiResult<iLineDetail> { TotalCount = totalCount, Values = values };
        }

        [HttpGet("api/v1/account/views")]
        [HttpGet("api/v1/account/portals/{portalId}/views")]
        [HttpGet("api/v1/account/catalogs/{catalogId}/views")]
        public async Task<ApiResult<iProduct>> Products(string orderBy = "EventDate", int page = 0, int pageSize = 50, int? catalogId = null, int? portalId = null)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sesionId = CartId();
            var query = _context.Views.Include(x => x.Product).Select(x => x);
            query = query.Where(x => x.UserId == userId);
            if (catalogId != null) query = query.Where(x => x.Product.CatalogId == catalogId);
            if (portalId != null) query = query.Include(x => x.Product.Catalog).Where(x => x.Product.Catalog.PortalId == portalId);
            query.OrderByDescending(x => x.EventDate);
            var totalCount = await query.CountAsync();
            query = query.Skip(page * pageSize).Take(pageSize);
            var values = await query.Select(x => x.Product).ToListAsync();
            return new ApiResult<iProduct> { TotalCount = totalCount, Values = values };
        }

    }
}

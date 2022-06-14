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


        public AccountController(RozetkadbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }


        [HttpPost("api/v1/users/{Id}/address/")]
        public async Task<ActionResult<iAddress>> PostAddress(string Id, [FromBody] Address address)
        {
            var newItem = new Address
            {
                AddressType = address.AddressType,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                UserId = Id,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country
            };
            _context.Addresses.Add(newItem);
            await _context.SaveChangesAsync();
            return newItem;
        }


        [HttpPut("api/v1/users/{Id}/Address/")]
        public async Task<ActionResult> PutAddress(string Id, [FromBody] Address address)
        {
            var newItem = await _context.Addresses.FindAsync(address.AddressId);
            if (newItem == null || newItem.UserId != Id)   return NotFound();  
            newItem.AddressType = address.AddressType?.ToString();
            newItem.AddressLine1 = address.AddressLine1?.ToString();
            newItem.AddressLine2 = address.AddressLine2?.ToString();
            newItem.AddressLine3 = address.AddressLine3?.ToString();  
            newItem.UserId = Id?.ToString();
            newItem.City = address.City?.ToString();      
            newItem.State = address.State?.ToString();        
            newItem.PostalCode = address.PostalCode?.ToString();       
            newItem.Country = address.Country?.ToString();
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("api/v1/users/{Id}/Address/")]
        public async Task<IActionResult> DeleteAddress(string Id,  Address address)
        {
            var newItem = await _context.Addresses.FindAsync(address.AddressId);
            if (newItem == null || newItem.UserId != Id) return NotFound();
            _context.Addresses.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("api/v1/users/{Id}/Contact/")]
        public async Task<ActionResult<iContact>> PostContact(string Id, [FromBody] Contact contact)
        {
            var newItem = new Contact
            {
                ContactType = contact.ContactType,
                DisplayName = contact.DisplayName,
                FullName = contact.FullName,
                Title = contact.Title,
                Salutation = contact.Salutation,
                UserId = Id,
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


        [HttpPut("api/v1/users/{Id}/Contact/")]
        public async Task<ActionResult> PutContact(string Id, [FromBody] Contact contact)
        {
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

        [HttpDelete("api/v1/users/{Id}/Contact/")]
        public async Task<IActionResult> DeleteContact(string Id, Contact Contact)
        {
            var newItem = await _context.Contacts.FindAsync(Contact.ContactId);
            if (newItem == null || newItem.UserId != Id) return NotFound();
            _context.Contacts.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPost("api/v1/users/{Id}/Wallett/")]
        public async Task<ActionResult<iWallett>> PostWallett(string Id, [FromBody] Wallett wallet)
        {
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


        [HttpPut("api/v1/users/{Id}/Wallett/")]
        public async Task<ActionResult> PutWallett(string Id, [FromBody] Wallett Wallett)
        {
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


        [HttpDelete("api/v1/users/{Id}/Wallett/")]
        public async Task<IActionResult> DeleteWallett(string Id, Wallett Wallett)
        {
            var newItem = await _context.Walletts.FindAsync(Wallett.WalletId);
            if (newItem == null || newItem.UserId != Id) return NotFound();
            _context.Walletts.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("api/v1/users/{Id}/Comment/")]
        public async Task<ActionResult<iComment>> PostComment(string Id, [FromBody] Comment Comment)
        {
            var newItem = new Comment
            {
                ProductId = Comment.ProductId,
                Text = Comment.Text,
                Date = Comment.Date,
                Score = Comment.Score,
                ImageId = Comment.ImageId,
                UserId = Id
            };
            _context.Comments.Add(newItem);
            await _context.SaveChangesAsync();
            return newItem;
        }


        [HttpPut("api/v1/users/{Id}/Comment/")]
        public async Task<ActionResult> PutComment(string Id, [FromBody] Comment Comment)
        {
            var newItem = await _context.Comments.FindAsync(Comment.CommentId);
            if (newItem == null || newItem.UserId != Id)  return NotFound();
            newItem.ProductId = Comment.ProductId;
            newItem.Text = Comment.Text?.ToString();
            newItem.Date = Comment.Date;
            newItem.Score = Comment.Score;
            newItem.ImageId = Comment.ImageId;
            newItem.UserId = Id;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("api/v1/users/{Id}/Comment/")]
        public async Task<IActionResult> DeleteComment(string Id, Comment Comment)
        {
            var newItem = await _context.Comments.FindAsync(Comment.CommentId);
            if (newItem == null || newItem.UserId != Id) return NotFound();
            _context.Comments.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPost("api/v1/users/{Id}/LineDetail/")]
        public async Task<ActionResult<iLineDetail>> PostLineDetail(string Id, [FromBody] LineDetail LineDetail)
        {
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


        [HttpPut("api/v1/users/{Id}/LineDetail/")]
        public async Task<ActionResult> PutLineDetail(string Id, [FromBody] LineDetail LineDetail)
        {
            var newItem = await _context.LineDetails.FindAsync(LineDetail.OrderDatailId);
            if (newItem == null || newItem.UserId != Id) return NotFound();
            newItem.ProductId = LineDetail.ProductId;
            newItem.Quantities = LineDetail.Quantities;
            newItem.UnitCost = _context.Products.Find(LineDetail.ProductId).Price;
            newItem.UserId = Id;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("api/v1/users/{Id}/LineDetail/")]
        public async Task<IActionResult> DeleteLineDetail(string Id, LineDetail LineDetail)
        {
            var newItem = await _context.LineDetails.FindAsync(LineDetail.OrderDatailId);
            if (newItem == null || newItem.UserId != Id) return NotFound();
            _context.LineDetails.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("api/v1/users/{Id}/Order/")]
        public async Task<ActionResult<iOrder>> PostOrder(string Id, [FromBody] Order Order)
        {
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
        }
   

        [HttpPost("api/v1/users/account")]
        public async Task<ActionResult<UserModel>> PostAspNetUser(string Id, [FromBody] UserModel Input)
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

        [HttpPut("api/v1/users/{Id}/account/")]
        public async Task<ActionResult<UserModel>> PutAspNetUser(string Id, [FromBody] UserModel Input)
        {
            var newItem = await _context.AspNetUsers.FindAsync(Input.Id);
            if (newItem == null || newItem.Id != Id) return NotFound();
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

        [HttpDelete("api/v1/users/{Id}/account/")]
        public async Task<IActionResult> DeleteAspNetUser(string Id, UserModel user)
        {
            var newItem = await _context.AspNetUsers.FindAsync(user.Id);
            if (newItem == null || newItem.Id != Id)return NotFound();
            _context.AspNetUsers.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

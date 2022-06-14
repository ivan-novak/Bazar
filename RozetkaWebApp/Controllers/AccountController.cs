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
    public class AccountController : Controller
    {
        private readonly RozetkadbContext _context;

        public AccountController(RozetkadbContext context)
        {
            _context = context;
        }

        [HttpPost("api/v1/[controller]/{Id}/Address/")]
        public async Task<ActionResult<iAddress>> PostAddress(string Id, [FromBody] iAddress address)
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


        [HttpPut("api/v1/[controller]/{Id}/Address/")]
        public async Task<ActionResult> PutAddress(string Id, [FromBody] iAddress address)
        {

            var newItem = await _context.Addresses.FindAsync(address.AddressId);
            if (newItem == null || newItem.UserId != Id)
            {
                return NotFound();
            }

            if (address.AddressType != null)
                newItem.AddressType = address.AddressType;
            if (address.AddressLine1 != null)
                newItem.AddressLine1 = address.AddressLine1;
            if (address.AddressLine2 != null)
                newItem.AddressLine2 = address.AddressLine2;
            if (address.AddressLine3 != null)
                newItem.AddressLine3 = address.AddressLine3;
            if (address.UserId != null)
                newItem.UserId = Id;
            if (address.City != null)
                newItem.City = address.City;
            if (address.State != null)
                newItem.State = address.State;
            if (address.PostalCode != null)
                newItem.PostalCode = address.PostalCode;
            if (address.Country != null)
                newItem.Country = address.Country;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("api/v1/[controller]/{Id}/Address/")]
        public async Task<IActionResult> DeleteAddress(string Id, [FromBody] iAddress address)
        {
            var newItem = await _context.Addresses.FindAsync(address.AddressId);

            if (newItem == null || newItem.UserId != Id)
            {
                return NotFound();
            }

            _context.Addresses.Remove(newItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("api/v1/[controller]/{Id}/Contact/")]
        public async Task<ActionResult<iContact>> PostContact(string Id, [FromBody] iContact contact)
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


        [HttpPut("api/v1/[controller]/{Id}/Contact/")]
        public async Task<ActionResult> PutContact(string Id, [FromBody] iContact contact)
        {
            var newItem = await _context.Contacts.FindAsync(contact.ContactId);
            if (newItem == null || newItem.UserId != Id)
            {
                return NotFound();
            }
            newItem.ContactType ??= contact.ContactType;
            newItem.DisplayName ??= contact.DisplayName;
            newItem.FullName ??= contact.FullName;
            newItem.Title ??= contact.Title;
            newItem.Salutation ??= contact.Salutation;
            newItem.UserId ??= Id;
            newItem.Attention ??= contact.Attention;
            newItem.FirstName ??= contact.FirstName;
            newItem.LastName ??= contact.LastName;
            newItem.Email ??= contact.Email;
            newItem.WebSite ??= contact.WebSite;
            newItem.Fax ??= contact.Fax;
            newItem.FaxType ??= contact.FaxType;
            newItem.Phone1 ??= contact.Phone1;
            newItem.Phone1Type ??= contact.Phone1Type;
            newItem.Phone2 ??= contact.Phone2;
            newItem.Phone2Type ??= contact.Phone2Type;
            newItem.Phone3 ??= contact.Phone3;
            newItem.Phone3Type ??= contact.Phone3Type;
            newItem.DefAddressId ??= contact.DefAddressId;
            newItem.AssignDate ??= contact.AssignDate;
            newItem.ExtAddressId ??= contact.ExtAddressId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("api/v1/[controller]/{Id}/Contact/")]
        public async Task<IActionResult> DeleteContact(string Id, iContact Contact)
        {
            var newItem = await _context.Contacts.FindAsync(Contact.ContactId);

            if (newItem == null || newItem.UserId != Id)
            {
                return NotFound();
            }
            _context.Contacts.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPost("api/v1/[controller]/{Id}/Wallett/")]
        public async Task<ActionResult<iWallett>> PostWallett(string Id, [FromBody] iWallett wallet)
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


        [HttpPut("api/v1/[controller]/{Id}/Wallett/")]
        public async Task<ActionResult> PutWallett(string Id, [FromBody] iWallett Wallett)
        {

            var newItem = await _context.Walletts.FindAsync(Wallett.WalletId);
            if (newItem == null || newItem.UserId != Id)
            {
                return NotFound();
            }
            newItem.CardType ??= Wallett.CardType;
            newItem.ExpiryDate ??= Wallett.ExpiryDate;
            newItem.Cardholder ??= Wallett.Cardholder;
            newItem.CardNumber ??= Wallett.CardNumber;
            newItem.UserId ??= Id;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("api/v1/[controller]/{Id}/Wallett/")]
        public async Task<IActionResult> DeleteWallett(string Id, iWallett Wallett)
        {
            var newItem = await _context.Walletts.FindAsync(Wallett.WalletId);

            if (newItem == null || newItem.UserId != Id)
            {
                return NotFound();
            }
            _context.Walletts.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("api/v1/[controller]/{Id}/Comment/")]
        public async Task<ActionResult<iComment>> PostComment(string Id, [FromBody] iComment Comment)
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


        [HttpPut("api/v1/[controller]/{Id}/Comment/")]
        public async Task<ActionResult> PutComment(string Id, [FromBody] iComment Comment)
        {
            var newItem = await _context.Comments.FindAsync(Comment.CommentId);
            if (newItem == null || newItem.UserId != Id)
            {
                return NotFound();
            }
            newItem.ProductId = Comment.ProductId;
            newItem.Text ??= Comment.Text;
            newItem.Date = Comment.Date;
            newItem.Score ??= Comment.Score;
            newItem.ImageId ??= Comment.ImageId;
            newItem.UserId ??= Id;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("api/v1/[controller]/{Id}/Comment/")]
        public async Task<IActionResult> DeleteComment(string Id, iComment Comment)
        {
            var newItem = await _context.Comments.FindAsync(Comment.CommentId);

            if (newItem == null || newItem.UserId != Id)
            {
                return NotFound();
            }
            _context.Comments.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPost("api/v1/[controller]/{Id}/LineDetail/")]
        public async Task<ActionResult<iLineDetail>> PostLineDetail(string Id, [FromBody] iLineDetail LineDetail)
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


        [HttpPut("api/v1/[controller]/{Id}/LineDetail/")]
        public async Task<ActionResult> PutLineDetail(string Id, [FromBody] iLineDetail LineDetail)
        {
            var newItem = await _context.LineDetails.FindAsync(LineDetail.OrderDatailId);
            if (newItem == null || newItem.UserId != Id)
            {
                return NotFound();
            }
            newItem.ProductId = LineDetail.ProductId;
            newItem.Quantities = LineDetail.Quantities;
            newItem.UnitCost = _context.Products.Find(LineDetail.ProductId).Price;
            newItem.UserId ??= Id;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("api/v1/[controller]/{Id}/LineDetail/")]
        public async Task<IActionResult> DeleteLineDetail(string Id, iLineDetail LineDetail)
        {
            var newItem = await _context.LineDetails.FindAsync(LineDetail.OrderDatailId);

            if (newItem == null || newItem.UserId != Id)
            {
                return NotFound();
            }
            _context.LineDetails.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("api/v1/[controller]/{Id}/Order/")]
        public async Task<ActionResult<iOrder>> PostOrder(string Id, [FromBody] iOrder Order)
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
            return newItem; }


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
            public bool EmailConfirmed { get; set; }
            public string NewPassword { get; set; }
            public string PhoneNumber { get; set; }
            public bool PhoneNumberConfirmed { get; set; }
            public bool? TwoFactorEnabled { get; set; }
         }


        [HttpPost("api/v1/[controller]/")]
        public async Task<ActionResult<UserModel>> PostAspNetUser(string Id, [FromBody] UserModel user)
        {
            var newItem = new AspNetUser
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber= user.PhoneNumber,
                TwoFactorEnabled= (bool)user.TwoFactorEnabled
            };
            _context.AspNetUsers.Add(newItem);
            await _context.SaveChangesAsync();
            user.Id = newItem.Id;
            return user;
        }


        [HttpPut("api/v1/[controller]/{Id}/User/")]
        public async Task<ActionResult> PutAspNetUser(string Id, [FromBody] UserModel user)
        {

            var newItem = await _context.AspNetUsers.FindAsync(user.Id);
            if (newItem == null || newItem.Id != Id)
            {
                return NotFound();
            }
            newItem.UserName ??= user.UserName;
            newItem.Email ??= user.Email;
            newItem.PhoneNumber ??= user.PhoneNumber;
            if (user.TwoFactorEnabled != null) newItem.TwoFactorEnabled = (bool)user.TwoFactorEnabled;
            newItem.Id = Id;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("api/v1/[controller]/{Id}/User/")]
        public async Task<IActionResult> DeleteAspNetUser(string Id, UserModel user)
        {
            var newItem = await _context.AspNetUsers.FindAsync(user.Id);
            if (newItem == null || newItem.Id != Id)
            {
                return NotFound();
            }
            _context.AspNetUsers.Remove(newItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

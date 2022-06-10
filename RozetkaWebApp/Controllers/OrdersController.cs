using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;

namespace RozetkaWebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly RozetkadbContext _context;

        public OrdersController(RozetkadbContext context)
        {
            _context = context;
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

        // GET: Orders
        //  [Authorize(Policy = "Owner")]
           public async Task<IActionResult> Index(string id)
        {
            if (id == null) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.User = _context.AspNetUsers.Find(id);
            var rozetkadbContext = _context.Orders.Where(x=>x.UserId==id).Include(o => o.User);
            return View(await rozetkadbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.User = _context.AspNetUsers.Find(order.UserId);
            ViewData["Lines"] = _context.LineDetails.Where(x => x.OrderId == order.OrderId).Include(x => x.Product).Select(x => x);
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create(string id = null)
        {
            if (id == null) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["WalletId"] = new SelectList(_context.Walletts.Where(a=>a.UserId == id), "WalletId", "CardNumber");
            ViewData["ContactId"] = new SelectList(_context.Contacts.Where(a => a.UserId == id), "ContactId", "FullName");
            ViewData["AddressId"] = new SelectList(_context.Addresses.Where(a => a.UserId == id), "AddressId", "FullAddress");
            ViewData["Cart"] = _context.LineDetails.Where(a => a.UserId == id && a.OrderId == null).Include(x=>x.Product).ToList();
            var a = ViewData["Cart"];
            ViewBag.User = _context.AspNetUsers.Find(id);
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,UserId,Description,Total,OrderDate,Status,CardNumber,DeliveryAddress,DeliveryContact,DeliveryEmail,DeliveryPhone,ExtOrderNbr")] Order order)
        {
            if (ModelState.IsValid)
            {
                int addressId = Int32.Parse(order.DeliveryAddress);
                int contactId = Int32.Parse(order.DeliveryContact);
                long walletId = Int64.Parse(order.CardNumber);

                var address = _context.Addresses.Find(addressId);
                var contact = _context.Contacts.Find(contactId);
                var wallet = _context.Walletts.Find(walletId);

                order.DeliveryAddress = address.FullAddress.ToString();
                order.DeliveryContact = contact.FullName.ToString();
                order.DeliveryEmail = contact.Email.ToString();
                order.DeliveryPhone = contact.Phone1.ToString();
                order.DeliveryPhone = contact.Phone1.ToString();
                order.CardNumber = wallet.CardNumber;
                order.OrderDate = DateTime.Now;
                var cart = _context.LineDetails.Where(a => (a.CartId == CartId() || a.UserId == order.UserId) && a.OrderId == null).Include(a=>a.Product).ToList();
                var description = String.Join(",", cart.Select(s => s.Product.Label));
                order.Description = description; 
                order.Total = cart.Sum(x => x.LineTotal);  
                _context.Add(order);
                await _context.SaveChangesAsync();

                foreach(var i in cart)
                {
                    i.OrderId = order.OrderId;
                    _context.Update(i);
                }
                await _context.SaveChangesAsync();

                return Redirect($"/Orders/Index/" + order.UserId);
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", order.UserId);
            return Redirect($"/Orders/Index/" + order.UserId);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", order.UserId);
            ViewBag.User = _context.AspNetUsers.Find(order.UserId);
            ViewData["Lines"] = _context.LineDetails.Where(x=>x.OrderId==order.OrderId).Include(x=>x.Product).Select(x=>x);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("OrderId,UserId,Description,Total,OrderDate,Status,CardNumber,DeliveryAddress,DeliveryContact,DeliveryEmail,DeliveryPhone,ExtOrderNbr")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/Orders/Index/" + order.UserId);
            }
            ViewData["Lines"] = _context.LineDetails.Where(x => x.OrderId == order.OrderId).Include(x => x.Product).Select(x => x);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", order.UserId);
            return Redirect($"/Orders/Index/" + order.UserId);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.User = _context.AspNetUsers.Find(order.UserId);
            ViewData["Lines"] = _context.LineDetails.Where(x => x.OrderId == order.OrderId).Include(x => x.Product).Select(x => x);
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return Redirect($"/Orders/Index/" + order.UserId);
        }

        private bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}

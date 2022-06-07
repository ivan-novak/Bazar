using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [Authorize]
    public class HomeController : Controller
    {
        private readonly RozetkadbContext _context;

        public HomeController(RozetkadbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Portals.ToListAsync());
        }

        public async Task<IActionResult> Carts()
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var rozetkadbContext = _context.LineDetails
                .Include(l => l.Order)
                .Include(l => l.Product)
                .Where(l => l.OrderId == null)
                .Where(l => l.CartId == CartId() || (l.UserId == userid));
            return View(await rozetkadbContext.ToListAsync());
        }


        public async Task<IActionResult> Catalogs(int? id)
        {
            return View(await _context.Catalogs.Where(x=>x.PortalId ==id || id ==null).Include(x=> x.Portal).ToListAsync());
        }

        public async Task<IActionResult> Products(int? id)
        {
            return View(await _context.Products.Where(x => x.CatalogId == id || id == null).Include(x => x.Catalog).ToListAsync());
        }

        public async Task<IActionResult> Characteristics(long? id)

        {
            var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartId = CartId();
            IQueryable<View> queryView = null;
            if (user_Id != null) queryView = _context.Views.Where(x => x.UserId == user_Id);
            else queryView = _context.Views.Where(x => x.CartId == cartId);
            View lineView = queryView.Where(x => x.ProductId == id).FirstOrDefault();
            if (lineView == null)
            {
                lineView = new View();
                lineView.CartId = CartId();
                lineView.UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                lineView.ProductId = id;
                lineView.EventDate = DateTime.Now;
                _context.Add(lineView);
            }
            else
            {
                lineView.EventDate = DateTime.Now;
                _context.Update(lineView);
            }
            await _context.SaveChangesAsync();
            ViewBag.Product = _context.Products.Find(id);
            return View(await _context.Characteristics.Where(x => x.ProductId == id || id == null).Include(x => x.Product).Include(x => x.Property).ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> AddToCart(long? id)
        {

            var product = _context.Products.Find(id);
            var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartId = CartId();
            var query = _context.LineDetails.Include(x => x.Product).Where(x => x.OrderId == null);
            if (user_Id != null) query = query.Where(x => x.UserId == user_Id);
            else query = query.Where(x => x.CartId == cartId);
            LineDetail lineDetail = query.Where(x=> x.ProductId == id).FirstOrDefault();
           if (lineDetail == null)
           {
                lineDetail = new LineDetail();
                lineDetail.CartId = CartId();
                lineDetail.UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                lineDetail.ProductId = (long)id;
                lineDetail.Quantities = 1;
                lineDetail.UnitCost = product.Price;
                _context.Add(lineDetail);
            }
            else
            {
                lineDetail.Quantities++;
                _context.Update(lineDetail);
            }
            await _context.SaveChangesAsync();
            return Redirect($"/Home/Characteristics/" + id.ToString());
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

        public IActionResult NewOrder()
        {
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewOrder([Bind("OrderId,UserId,Description,Total,OrderDate,Status,CardNumber,DeliveryAddress,DeliveryContact,DeliveryEmail,DeliveryPhone,ExtOrderNbr")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", order.UserId);
            return View(order);
        }
    }


}

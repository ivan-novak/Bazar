//MLHIDEFILE
using System;
using System.Collections.Generic;
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
    public class AddressesController : Controller
    {
        private readonly RozetkadbContext _context;

        public AddressesController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index(string id)
        {
            if (id == null) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.User = _context.AspNetUsers.Find(id);
            var rozetkadbContext = _context.Addresses.Include(a => a.User).Where(a=>a.UserId == id);
            return View(await rozetkadbContext.ToListAsync());
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (userid == null) return Redirect($"/Identity/Account/Register");
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }
            ViewBag.User = _context.AspNetUsers.Find(address.UserId);
            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create(string id = null)
        {
            if (id == null) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.User = _context.AspNetUsers.Find(id);
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AddressId,AddressType,AddressLine1,AddressLine2,AddressLine3,UserId,City,State,PostalCode,Country,ExtAddressId")] Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return Redirect($"/Addresses/Index/" + address.UserId);
            }
            return Redirect($"/Addresses/Index/" + address.UserId);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            ViewBag.User = _context.AspNetUsers.Find(address.UserId);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AddressId,AddressType,AddressLine1,AddressLine2,AddressLine3,UserId,City,State,PostalCode,Country,ExtAddressId")] Address address)
        {
            if (id != address.AddressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.AddressId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/Addresses/Index/" + address.UserId);
            }
            return Redirect($"/Addresses/Index/" + address.UserId);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }
            ViewBag.User = _context.AspNetUsers.Find(address.UserId);
            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return Redirect($"/Addresses/Index/" + address.UserId);
        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.AddressId == id);
        }
    }
}

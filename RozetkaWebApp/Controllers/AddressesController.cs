//MLHIDEFILE
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RozetkaWebApp.Controllers
{
    [Authorize(Roles = "Користувачі")]
    public class AddressesController : Controller
    {
        private readonly RozetkadbContext _context;

        public AddressesController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index(string id, string Filter = null, int page = 0, int pageSize = 20)
        {
            if (id == null) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.User = _context.AspNetUsers.Find(id);
            var rozetkadbContext = _context.Addresses.Include(a => a.User).Where(a => a.UserId == id);

            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = rozetkadbContext.Where(x => Filter == null || x.FullAddress.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.AddressLine1).Skip(pageSize * page).Take(pageSize).ToListAsync());
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
        [Authorize(Roles = "Продавці")]

        // GET: Addresses/Create
        public IActionResult Create(string id = null)
        {
            if (id == null) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.User = _context.AspNetUsers.Find(id);
            return View();
        }
        [Authorize(Roles = "Продавці")]

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
        [Authorize(Roles = "Продавці")]

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
        [Authorize(Roles = "Продавці")]

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
        [Authorize(Roles = "Продавці")]

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
        [Authorize(Roles = "Продавці")]

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

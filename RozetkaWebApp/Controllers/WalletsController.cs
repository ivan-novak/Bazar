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
    public class WalletsController : Controller
    {
        private readonly RozetkadbContext _context;

        public WalletsController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Walletts
        public async Task<IActionResult> Index(string id)
        {
            if (id == null) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["User"] = _context.AspNetUsers.Find(id);
            var rozetkadbContext = _context.Walletts.Include(a => a.User).Where(a => a.UserId == id);
            return View(await rozetkadbContext.ToListAsync());
        }

        // GET: Walletts/Details/5
        public async Task<IActionResult> Details(long? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var wallett = await _context.Walletts
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.WalletId == id);
            if (wallett == null)
            {
                return NotFound();
            }
            ViewData["User"] = _context.AspNetUsers.Find(wallett.UserId);
            return View(wallett);
        }

        // GET: Walletts/Create
        public IActionResult Create(string id)
        {
            if (id == null) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["User"] = _context.AspNetUsers.Find(id);
            return View();
        }

        // POST: Walletts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WalletId,CardNumber,ExpiryDate,Cardholder,CardType,VerificationCode,UserId")] Wallett wallett)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wallett);
                await _context.SaveChangesAsync();
                return Redirect($"/Wallets/Index/" + wallett.UserId);
            }
            return Redirect($"/Wallets/Index/" + wallett.UserId);
        }

        // GET: Walletts/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallett = await _context.Walletts.FindAsync(id);
            if (wallett == null)
            {
                return NotFound();
            }
            ViewData["User"] = _context.AspNetUsers.Find(wallett.UserId);
            return View(wallett);
        }

        // POST: Walletts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("WalletId,CardNumber,ExpiryDate,Cardholder,CardType,VerificationCode,UserId")] Wallett wallett)
        {
            if (id != wallett.WalletId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wallett);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WallettExists(wallett.WalletId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/Wallets/Index/" + wallett.UserId);
            }
            return Redirect($"/Wallets/Index/" + wallett.UserId);
        }

        // GET: Walletts/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallett = await _context.Walletts
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.WalletId == id);
            if (wallett == null)
            {
                return NotFound();
            }
            ViewData["User"] = _context.AspNetUsers.Find(wallett.UserId);
            return View(wallett);

        }

        // POST: Walletts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var wallett = await _context.Walletts.FindAsync(id);
            _context.Walletts.Remove(wallett);
            await _context.SaveChangesAsync();
            return Redirect($"/Wallets/Index/" + wallett.UserId);
        }

        private bool WallettExists(long id)
        {
            return _context.Walletts.Any(e => e.WalletId == id);
        }
    }
}

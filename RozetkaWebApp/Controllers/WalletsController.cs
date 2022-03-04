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
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
            var rozetkadbContext = _context.Walletts.Include(a => a.User).Where(a => a.UserId == userid);
            return View(await rozetkadbContext.ToListAsync());
        }

        // GET: Walletts/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
            if (id == null)
            {
                return NotFound();
            }

            var wallett = await _context.Walletts
                .Include(w => w.User).Where(a => a.UserId == userid)
                .FirstOrDefaultAsync(m => m.WalletId == id);
            if (wallett == null)
            {
                return NotFound();
            }

            return View(wallett);
        }

        // GET: Walletts/Create
        public IActionResult Create()
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
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
                var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userid == null) return Redirect($"/Identity/Account/Register");
                wallett.UserId = userid;
                _context.Add(wallett);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", wallett.UserId);
            return View(wallett);
        }

        // GET: Walletts/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
            if (id == null)
            {
                return NotFound();
            }

            var wallett = await _context.Walletts.FindAsync(id);
            if (wallett == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", wallett.UserId);
            return View(wallett);
        }

        // POST: Walletts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("WalletId,CardNumber,ExpiryDate,Cardholder,CardType,VerificationCode,UserId")] Wallett wallett)
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
            if (id != wallett.WalletId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    wallett.UserId = userid;
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", wallett.UserId);
            return View(wallett);
        }

        // GET: Walletts/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
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
            return RedirectToAction(nameof(Index));
        }

        private bool WallettExists(long id)
        {
            return _context.Walletts.Any(e => e.WalletId == id);
        }
    }
}

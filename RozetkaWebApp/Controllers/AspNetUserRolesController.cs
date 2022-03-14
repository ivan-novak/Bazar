using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;

namespace RozetkaWebApp.Controllers
{
    public class AspNetUserRolesController : Controller
    {
        private readonly RozetkadbContext _context;

        public AspNetUserRolesController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: AspNetUserRoles
        public async Task<IActionResult> Index(string id)
        {
            ViewData["Role"] = _context.AspNetRoles.Find(id);
            var rozetkadbContext = _context.AspNetUserRoles.Where(a=>a.RoleId == id).Include(a => a.Role).Include(a => a.User);
            return View(await rozetkadbContext.ToListAsync());
        }

        // GET: AspNetUserRoles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUserRole = await _context.AspNetUserRoles
                .Include(a => a.Role)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (aspNetUserRole == null)
            {
                return NotFound();
            }

            return View(aspNetUserRole);
        }

        // GET: AspNetUserRoles/Create
        public IActionResult Create(string Id)
        {
            ViewData["Role"] = _context.AspNetRoles.Find(Id);
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            return View();
        }

        // POST: AspNetUserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId")] AspNetUserRole aspNetUserRole)
        {
            ViewData["Role"] = _context.AspNetRoles.Find(aspNetUserRole.RoleId);
            if (ModelState.IsValid)
            {
                _context.Add(aspNetUserRole);
                await _context.SaveChangesAsync();
                return Redirect($"/AspNetUserRoles/Index/" + aspNetUserRole.RoleId);
            }
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRole.UserId);
            return Redirect($"/AspNetUserRoles/Index/" + aspNetUserRole.RoleId);
        }

        // GET: AspNetUserRoles/Edit/5
        public async Task<IActionResult> Edit(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var aspNetUserRole = await _context.AspNetUserRoles.Where(a=>a.UserId+a.RoleId == Id).FirstOrDefaultAsync();
            if (aspNetUserRole == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }

        // POST: AspNetUserRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,RoleId")] AspNetUserRole aspNetUserRole)
        {
            if (id != aspNetUserRole.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUserRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserRoleExists(aspNetUserRole.UserId))
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
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }

        // GET: AspNetUserRoles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUserRole = await _context.AspNetUserRoles
                .Include(a => a.Role)
                .Include(a => a.User)
                .Where(a => a.UserId + a.RoleId == id).FirstOrDefaultAsync();
            if (aspNetUserRole == null)
            {
                return NotFound();
            }
            ViewData["Role"] = _context.AspNetRoles.Find(aspNetUserRole.RoleId);
            return View(aspNetUserRole);
        }

        // POST: AspNetUserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetUserRole = await _context.AspNetUserRoles.Where(a => a.UserId + a.RoleId == id).FirstOrDefaultAsync();

            ViewData["Role"] = _context.AspNetRoles.Find(aspNetUserRole.RoleId);
            _context.AspNetUserRoles.Remove(aspNetUserRole);
            await _context.SaveChangesAsync();
            return Redirect($"/AspNetUserRoles/Index/" + aspNetUserRole.RoleId);
        }

        private bool AspNetUserRoleExists(string id)
        {
            return _context.AspNetUserRoles.Any(e => e.UserId == id);
        }
    }
}

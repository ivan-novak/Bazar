//MLHIDEFILE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;

namespace RozetkaWebApp
{
    [Authorize(Roles = "Користувачі")]
    public class PortalsController : Controller
    {
        private readonly RozetkadbContext _context;

        public PortalsController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Portals
        [HttpGet("[controller]/Index")]
        public async Task<IActionResult> Index(string Filter = null, int page = 0, int pageSize = 20)
        {
            var applicationDbContext = _context.Portals;

           ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = applicationDbContext.Where(x => Filter == null || x.Label.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.Label).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }

        // GET: Portals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portal = await _context.Portals.Include(m => m.Catalogs)
                .FirstOrDefaultAsync(m => m.PortalId == id);
            if (portal == null)
            {
                return NotFound();
            }

            return View(portal);
        }

        // GET: Portals/Create
        [Authorize(Roles = "Маркетологи")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Portals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PortalId,Title,Label,Description,Attributes")] Portal portal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(portal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(portal);
        }

        [Authorize(Roles = "Маркетологи")]
        // GET: Portals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portal = await _context.Portals.FindAsync(id);
            if (portal == null)
            {
                return NotFound();
            }
            return View(portal);
        }

        // POST: Portals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Маркетологи")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PortalId,Title,Label,Description,Attributes")] Portal portal)
        {
            if (id != portal.PortalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(portal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortalExists(portal.PortalId))
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
            return View(portal);
        }

        [Authorize(Roles = "Маркетологи")]
        // GET: Portals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portal = await _context.Portals
                .FirstOrDefaultAsync(m => m.PortalId == id);
            if (portal == null)
            {
                return NotFound();
            }

            return View(portal);
        }

        [Authorize(Roles = "Маркетологи")]
        // POST: Portals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var portal = await _context.Portals.FindAsync(id);
            _context.Portals.Remove(portal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PortalExists(int id)
        {
            return _context.Portals.Any(e => e.PortalId == id);
        }
    }
}

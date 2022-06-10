//MLHIDEFILE
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
    public class PropertiesController : Controller
    {
        private readonly RozetkadbContext _context;

        public PropertiesController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Properties
        public async Task<IActionResult> Index(int? id, string Filter = null, int page = 0, int pageSize = 20)
        {
            if (id != null) ViewBag.Catalog = _context.Catalogs.Include(c => c.Portal).First(i => i.CatalogId == id);
            var applicationDbContext = _context.Properties.Where(c => c.CatalogId == id || id == null).Include(c => c.Catalog);
            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = applicationDbContext.Where(x => Filter == null || x.Label.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.Label).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(pp => pp.Catalog)
                .FirstOrDefaultAsync(m => m.PropertyId == id);
            if (property == null)
            {
                return NotFound();
            }
            ViewBag.Catalog = _context.Catalogs.Include(c => c.Portal).First(i => i.CatalogId == property.CatalogId);
            return View(property);
        }

        // GET: Properties/Create
        public IActionResult Create(int? Id)
        {
            var catalog = _context.Catalogs.Include(c => c.Portal).FirstOrDefault(m => m.CatalogId == Id);
            ViewBag.Catalog = catalog;
            ViewBag.CatalogId = new SelectList(_context.Catalogs.Where(i => i.PortalId == catalog.PortalId), "CatalogId", "Label");
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PropertyId,CatalogId,Title,Label,Format,IsNumber,Description")] Property @property)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@property);
                await _context.SaveChangesAsync();
                return Redirect($"/Properties/Index/" + @property.CatalogId);
              //  return RedirectToAction(nameof(Index));
            }
            ViewBag.CatalogId = new SelectList(_context.Catalogs, "CatalogId", "Label", @property.CatalogId);
            return View(@property);
        }

        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            ViewBag.Catalog = _context.Catalogs.Include(c => c.Portal).First(i => i.CatalogId == property.CatalogId);
            ViewBag.CatalogId = new SelectList(_context.Catalogs.Where(i => i.PortalId == property.Catalog.PortalId), "CatalogId", "Label");
            return View(property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PropertyId,CatalogId,Title,Label, Category, Format, Mask, IsNumber,Description")] Property @property)
        {
            if (id != @property.PropertyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(@property.PropertyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/Properties/Index/" + @property.CatalogId);
            }
            ViewBag.CatalogId = new SelectList(_context.Catalogs, "CatalogId", "Label", @property.CatalogId);
            return View(@property);
        }

        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(pp => pp.Catalog)
                .FirstOrDefaultAsync(m => m.PropertyId == id);
            if (property == null)
            {
                return NotFound();
            }
            ViewBag.Catalog = _context.Catalogs.Include(c => c.Portal).First(i => i.CatalogId == property.CatalogId);
            return View(property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @property = await _context.Properties.FindAsync(id);
            id = @property.CatalogId;
            _context.Properties.Remove(@property);
            await _context.SaveChangesAsync();
            return Redirect($"/Properties/Index/" + id);
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.PropertyId == id);
        }
    }
}

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
    public class CatalogImages1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogImages1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CatalogImages1
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CatalogImage.Include(c => c.Catalog);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CatalogImages1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalogImage = await _context.CatalogImage
                .Include(c => c.Catalog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catalogImage == null)
            {
                return NotFound();
            }

            return View(catalogImage);
        }

        // GET: CatalogImages1/Create
        public IActionResult Create()
        {
            ViewData["CatalogId"] = new SelectList(_context.Catalog, "CatalogId", "CatalogId");
            return View();
        }

        // POST: CatalogImages1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CatalogId,Title,Label,Data")] CatalogImage catalogImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catalogImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatalogId"] = new SelectList(_context.Catalog, "CatalogId", "CatalogId", catalogImage.CatalogId);
            return View(catalogImage);
        }

        // GET: CatalogImages1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalogImage = await _context.CatalogImage.FindAsync(id);
            if (catalogImage == null)
            {
                return NotFound();
            }
            ViewData["CatalogId"] = new SelectList(_context.Catalog, "CatalogId", "CatalogId", catalogImage.CatalogId);
            return View(catalogImage);
        }

        // POST: CatalogImages1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CatalogId,Title,Label")] CatalogImage catalogImage)
        {
            if (id != catalogImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catalogImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatalogImageExists(catalogImage.Id))
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
            ViewData["CatalogId"] = new SelectList(_context.Catalog, "CatalogId", "CatalogId", catalogImage.CatalogId);
            return View(catalogImage);
        }

        // GET: CatalogImages1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalogImage = await _context.CatalogImage
                .Include(c => c.Catalog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catalogImage == null)
            {
                return NotFound();
            }

            return View(catalogImage);
        }

        // POST: CatalogImages1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catalogImage = await _context.CatalogImage.FindAsync(id);
            _context.CatalogImage.Remove(catalogImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatalogImageExists(int id)
        {
            return _context.CatalogImage.Any(e => e.Id == id);
        }
    }
}

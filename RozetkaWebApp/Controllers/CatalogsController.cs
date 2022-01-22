using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;

namespace RozetkaWebApp
{
    public class CatalogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogsController(ApplicationDbContext context)
        {
            _context = context;
        }




        // GET: Catalogs
        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.Portal = _context.Portal.Find(id);

            var applicationDbContext = _context.Catalog.Where(c=>c.PortalId == id || id == null).Include(c => c.Portal);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Catalogs/Image/5

        public async Task<FileResult> Image(int? id)
        {
            if (id == null) return null;
            var image = await _context.CatalogImage.FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)  return null;
            System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(image.Data);
            return new FileStreamResult(oMemoryStream, "image/*");
        }

        // GET: Catalogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalog
                .Include(c => c.Portal)
                .FirstOrDefaultAsync(m => m.CatalogId == id);
            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }

        // GET: Catalogs/Create
        public IActionResult Create(int? Id)
        {
            var catalog = _context.Portal.FirstOrDefault(m => m.PortalId == Id);
            ViewBag.Portal = catalog;
            ViewData["PortalId"] = new SelectList(_context.Portal, "PortalId", "Label");
            return View();
        }

        // POST: Catalogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatalogId,PortalId,Title,Label,Description,Attributes")] Catalog catalog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catalog);
                await _context.SaveChangesAsync();
                return Redirect($"/Catalogs/Index/" + catalog.PortalId);
             //   return RedirectToAction(nameof(Index));
            }
            ViewData["PortalId"] = new SelectList(_context.Portal, "PortalId", "Label", catalog.PortalId);
            return View(catalog);
        }

        // GET: Catalogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalog
                .Include(c => c.Portal)
                .FirstOrDefaultAsync(m => m.CatalogId == id);
            if (catalog == null)
            {
                return NotFound();
            }
            ViewData["PortalId"] = new SelectList(_context.Portal, "PortalId", "Label", catalog.PortalId);
            return View(catalog);
        }

        // POST: Catalogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatalogId,PortalId,Title,Label,Description,Attributes")] Catalog catalog)
        {
            if (id != catalog.CatalogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catalog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatalogExists(catalog.CatalogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/Catalogs/Index/" + catalog.PortalId);
                return RedirectToAction(nameof(Index));
            }
            ViewData["PortalId"] = new SelectList(_context.Portal, "PortalId", "Label", catalog.PortalId);
            return View(catalog);
        }

        // GET: Catalogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalog
                .Include(c => c.Portal)
                .FirstOrDefaultAsync(m => m.CatalogId == id);
            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }

        // POST: Catalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catalog = await _context.Catalog.FindAsync(id);
            var Id = catalog.PortalId;

            _context.Catalog.Remove(catalog);
            await _context.SaveChangesAsync();
            return Redirect($"/Catalogs/Index/" + Id);
            return RedirectToAction(nameof(Index));
        }

        private bool CatalogExists(int id)
        {
            return _context.Catalog.Any(e => e.CatalogId == id);
        }
    }
}

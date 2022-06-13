//MLHIDEFILE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

    public class CatalogsController : Controller
    {
        private readonly RozetkadbContext _context;

        public CatalogsController(RozetkadbContext context)
        {
            _context = context;
        }




        // GET: Catalogs
          public async Task<IActionResult> Index(int? id, string Filter = null, int page = 0, int pageSize = 20)
        {
            ViewBag.Portal = _context.Portals.Find(id);

            var applicationDbContext = _context.Catalogs.Where(c=>c.PortalId == id || id == null).Include(c => c.Portal);
            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = applicationDbContext.Where(x => Filter == null || x.Label.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.Label).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }
    

        // GET: Catalogs/Image/5

        public async Task<FileResult> Image(int? id)
        {
            if (id == null) return null;
            var image = await _context.CatalogImages.FirstOrDefaultAsync(m => m.Id == id);
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

            var catalog = await _context.Catalogs
                .Include(c => c.Portal)
                .FirstOrDefaultAsync(m => m.CatalogId == id);
            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }

        [Authorize(Roles = "Маркетологи")]

        // GET: Catalogs/Create
        public IActionResult Create(int? Id)
        {
            var catalog = _context.Portals.FirstOrDefault(m => m.PortalId == Id);
            ViewBag.Portal = catalog;
            ViewBag.PortalId = new SelectList(_context.Portals, "PortalId", "Label");
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
            ViewBag.PortalId = new SelectList(_context.Portals, "PortalId", "Label", catalog.PortalId);
            return View(catalog);
        }

        [Authorize(Roles = "Маркетологи")]

        // GET: Catalogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalogs
                .Include(c => c.Portal)
                .FirstOrDefaultAsync(m => m.CatalogId == id);
            if (catalog == null)
            {
                return NotFound();
            }
            ViewBag.PortalId = new SelectList(_context.Portals, "PortalId", "Label", catalog.PortalId);
            return View(catalog);
        }

        [Authorize(Roles = "Маркетологи")]

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
            ViewBag.PortalId = new SelectList(_context.Portals, "PortalId", "Label", catalog.PortalId);
            return View(catalog);
        }


        [Authorize(Roles = "Маркетологи")]

        // GET: Catalogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalogs
                .Include(c => c.Portal)
                .FirstOrDefaultAsync(m => m.CatalogId == id);
            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }


        [Authorize(Roles = "Маркетологи")]

        // POST: Catalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catalog = await _context.Catalogs.FindAsync(id);
            var Id = catalog.PortalId;

            _context.Catalogs.Remove(catalog);
            await _context.SaveChangesAsync();
            return Redirect($"/Catalogs/Index/" + Id);
            return RedirectToAction(nameof(Index));
        }

        private bool CatalogExists(int id)
        {
            return _context.Catalogs.Any(e => e.CatalogId == id);
        }

        //[HttpGet("[controller]/{id}/image/{name}")]
        //public async Task<FileResult> Index(long? id, string name)
        //{
        //    if (id == null || name == null) return null;
        //    var catalogImage = await _context.CatalogImages.Where(p => p.CatalogId == id && p.Label == name).FirstOrDefaultAsync();
        //    if (catalogImage == null) return null;
        //    var image = await _context.Images.FirstOrDefaultAsync(m => m.ImageId == catalogImages.ImageId);
        //    if (image == null) return null;
        //    return Images.ToStream();
        //}
    }
}

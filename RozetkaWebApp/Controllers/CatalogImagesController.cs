using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;

namespace RozetkaWebApp.Controllers
{
    public class CatalogImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CatalogImages
        public async Task<IActionResult> Index(int? id)
        {
            if (id != null) ViewBag.Catalog = _context.Catalog.Where(i => i.CatalogId == id).Include(c => c.Portal).First();
            var applicationDbContext = _context.CatalogImage.Where(c => c.CatalogId == id || id == null).Include(c => c.Catalog);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CatalogImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)  return NotFound();
            var catalogImage = await _context.CatalogImage.Where(m => m.Id == id).Include(c => c.Catalog).FirstAsync();
            if (catalogImage == null)  return NotFound();
            return View(catalogImage);
        }

        // GET: CatalogImages/Create
        public IActionResult Create(int? Id)
        {
            var catalog = _context.Catalog.Where(m => m.CatalogId == Id).Include(c => c.Portal).First();
            ViewBag.Catalog = catalog;
            return View();
        }

        // POST: CatalogImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatalogId,Title,Label,Data")] CatalogImage catalogImage)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in Request.Form.Files)
                {
                    var image = new Image();
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    image.Data = ms.ToArray();
                    image.Title = file.FileName;
                    _context.Add(image);
                    await _context.SaveChangesAsync();
                    catalogImage.ImageId = image.ImageId;
                    ms.Close();
                    ms.Dispose();
                }
                _context.Add(catalogImage);
                await _context.SaveChangesAsync();
                return Redirect($"/CatalogImages/Index/" + catalogImage.CatalogId);
            }
            return View(catalogImage);
        }

        // GET: CatalogImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            CatalogImage catalogImage = await _context.CatalogImage.FindAsync(id);
            if (catalogImage == null)  return NotFound();
            ViewBag.Catalog = _context.Catalog.Where(i => i.CatalogId == catalogImage.CatalogId).Include(i=>i.Portal).First();
            return View(catalogImage);
        }

        // POST: CatalogImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CatalogId,ImageId,Title,Label,Data")] CatalogImage catalogImage)
        {
            if (id != catalogImage.Id)  return NotFound();
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                var image = _context.Image.Find(catalogImage.ImageId);
                image.Data = ms.ToArray();
                image.Title = file.FileName;
                _context.Update(image);
                ms.Close();
                ms.Dispose();
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
                    if (!CatalogImageExists(catalogImage.Id)) return NotFound();
                    else  throw;
                }
                return Redirect($"/CatalogImages/Index/" + catalogImage.CatalogId);
            }
            return View(catalogImage);
        }

        // GET: CatalogImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)  return NotFound();
            var catalogImage = await _context.CatalogImage.Where(m => m.Id == id).FirstAsync();
            if (catalogImage == null) return NotFound();
            ViewBag.Catalog = _context.Catalog.Where(i => i.CatalogId == catalogImage.CatalogId).Include(c => c.Portal).First();
            return View(catalogImage);
        }

        // POST: CatalogImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catalogImage = await _context.CatalogImage.FindAsync(id);
            _context.CatalogImage.Remove(catalogImage);
            await _context.SaveChangesAsync();
            return Redirect($"/CatalogImages/Index/" + catalogImage.CatalogId);
        }

        private bool CatalogImageExists(int id)
        {
            return _context.CatalogImage.Any(e => e.Id == id);
        }
    }
}

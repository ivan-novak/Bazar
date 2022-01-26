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
    public class RootImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RootImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RootImages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RootImage.Include(r => r.Image);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RootImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rootImage = await _context.RootImage
                .Include(r => r.Image)
                .FirstOrDefaultAsync(m => m.RootImageId == id);
            if (rootImage == null)
            {
                return NotFound();
            }

            return View(rootImage);
        }

        // GET: RootImages/Create
        public IActionResult Create()
        {
            ViewData["ImageId"] = new SelectList(_context.Image, "ImageId", "ImageId");
            return View();
        }

        // POST: RootImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RootImageId,Title,Label,ImageId")] RootImage rootImage)
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
                    rootImage.ImageId = image.ImageId;
                    ms.Close();
                    ms.Dispose();
                }


                _context.Add(rootImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ImageId"] = new SelectList(_context.Image, "ImageId", "ImageId", rootImage.ImageId);
            return View(rootImage);
        }

        // GET: RootImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rootImage = await _context.RootImage.FindAsync(id);
            if (rootImage == null)
            {
                return NotFound();
            }
            ViewData["ImageId"] = new SelectList(_context.Image, "ImageId", "ImageId", rootImage.ImageId);
            return View(rootImage);
        }

        // POST: RootImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RootImageId,Title,Label,ImageId")] RootImage rootImage)
        {
            if (id != rootImage.RootImageId)
            {
                return NotFound();
            }

            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                var image = _context.Image.Find(rootImage.ImageId);
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
                    _context.Update(rootImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RootImageExists(rootImage.RootImageId))
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
            ViewData["ImageId"] = new SelectList(_context.Image, "ImageId", "ImageId", rootImage.ImageId);
            return View(rootImage);
        }

        // GET: RootImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rootImage = await _context.RootImage
                .Include(r => r.Image)
                .FirstOrDefaultAsync(m => m.RootImageId == id);
            if (rootImage == null)
            {
                return NotFound();
            }

            return View(rootImage);
        }

        // POST: RootImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rootImage = await _context.RootImage.Where(m => m.RootImageId == id).FirstAsync();
            _context.RootImage.Remove(rootImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RootImageExists(int id)
        {
            return _context.RootImage.Any(e => e.RootImageId == id);
        }
    }
}

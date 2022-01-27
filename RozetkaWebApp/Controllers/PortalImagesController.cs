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
    public class PortalImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PortalImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PortalImages
        public async Task<IActionResult> Index(int? id)
        {
            if (id != null) ViewBag.Portal = _context.Portal.Where(i => i.PortalId == id).First();
            var applicationDbContext = _context.PortalImage.Where(c => c.PortalId == id || id == null);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PortalImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portalImage = await _context.PortalImage
                .FirstOrDefaultAsync(m => m.PortalImageId == id);
            if (portalImage == null)
            {
                return NotFound();
            }

            return View(portalImage);
        }

        // GET: PortalImages/Create
        public IActionResult Create(int? Id)
        {
            var portal = _context.Portal.Where(m => m.PortalId == Id).First();
            ViewBag.Portal = portal;
            return View();
        }

        // POST: PortalImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PortalImageId,PortalId,Title,Label,ImageId")] PortalImage portalImage)
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
                    portalImage.ImageId = image.ImageId;
                    ms.Close();
                    ms.Dispose();
                }
                _context.Add(portalImage);
                await _context.SaveChangesAsync();
                return Redirect($"/PortalImages/Index/" + portalImage.PortalId);
               // return RedirectToAction(nameof(Index));
            }
            return View(portalImage);
        }

        // GET: PortalImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            PortalImage portalImage = await _context.PortalImage.FindAsync(id);
            if (portalImage == null) return NotFound();
            ViewBag.Portal = _context.Portal.Where(i => i.PortalId == portalImage.PortalId).First();
            return View(portalImage);
        }

        // POST: PortalImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PortalImageId,PortalId,Title,Label,ImageId")] PortalImage portalImage)
        {
            if (id != portalImage.PortalImageId)
            {
                return NotFound();
            }
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                var image = _context.Image.Find(portalImage.ImageId);
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
                    _context.Update(portalImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortalImageExists(portalImage.PortalImageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/PortalImages/Index/" + portalImage.PortalId);
            }
            return View(portalImage);
        }

        // GET: PortalImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var portalImage = await _context.PortalImage.FirstOrDefaultAsync(m => m.PortalImageId == id);
            if (portalImage == null) return NotFound();
            ViewBag.Portal = _context.Portal.Where(i => i.PortalId == portalImage.PortalId).First();

            return View(portalImage);
        }

        // POST: PortalImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var portalImage = await _context.PortalImage.FindAsync(id);
            _context.PortalImage.Remove(portalImage);
            await _context.SaveChangesAsync();
            return Redirect($"/PortalImages/Index/" + portalImage.PortalId);
        }

        private bool PortalImageExists(int id)
        {
            return _context.PortalImage.Any(e => e.PortalImageId == id);
        }

        [HttpGet("[controller]/{id}/image/{name}")]
        public async Task<FileResult> Image(long? id, string name)
        {
            if (id == null || name == null) return null;
            var portalImage = await _context.PortalImage.Where(p => p.PortalId == id && p.Label == name).FirstOrDefaultAsync();
            if (portalImage == null) return null;
            var image = await _context.Image.FirstOrDefaultAsync(m => m.ImageId == portalImage.ImageId);
            if (image == null) return null;
            return image.ToStream();
        }
    }
}

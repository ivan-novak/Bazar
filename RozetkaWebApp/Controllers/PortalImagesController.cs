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
    public class PortalImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PortalImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PortalImages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PortalImage.Include(p => p.Portal);
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
                .Include(p => p.Portal)
                .FirstOrDefaultAsync(m => m.PortalImageId == id);
            if (portalImage == null)
            {
                return NotFound();
            }

            return View(portalImage);
        }

        // GET: PortalImages/Create
        public IActionResult Create()
        {
            ViewData["PortalId"] = new SelectList(_context.Portal, "PortalId", "Title");
            return View();
        }

        // POST: PortalImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PortalImageId,PortalId,Caption,Path")] PortalImage portalImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(portalImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PortalId"] = new SelectList(_context.Portal, "PortalId", "Title", portalImage.PortalId);
            return View(portalImage);
        }

        // GET: PortalImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portalImage = await _context.PortalImage.FindAsync(id);
            if (portalImage == null)
            {
                return NotFound();
            }
            ViewData["PortalId"] = new SelectList(_context.Portal, "PortalId", "Title", portalImage.PortalId);
            return View(portalImage);
        }

        // POST: PortalImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PortalImageId,PortalId,Caption,Path")] PortalImage portalImage)
        {
            if (id != portalImage.PortalImageId)
            {
                return NotFound();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["PortalId"] = new SelectList(_context.Portal, "PortalId", "Title", portalImage.PortalId);
            return View(portalImage);
        }

        // GET: PortalImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portalImage = await _context.PortalImage
                .Include(p => p.Portal)
                .FirstOrDefaultAsync(m => m.PortalImageId == id);
            if (portalImage == null)
            {
                return NotFound();
            }

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
            return RedirectToAction(nameof(Index));
        }

        private bool PortalImageExists(int id)
        {
            return _context.PortalImage.Any(e => e.PortalImageId == id);
        }
    }
}

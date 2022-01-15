using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;

namespace RozetkaWebApp
{
    public class ControlImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ControlImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ControlImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.ControlImage.ToListAsync());
        }

        // GET: ControlImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controlImage = await _context.ControlImage
                .FirstOrDefaultAsync(m => m.ControlImageId == id);
            if (controlImage == null)
            {
                return NotFound();
            }

            return View(controlImage);
        }

        // GET: ControlImages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ControlImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ControlImageId,Title,Label,Path")] ControlImage controlImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(controlImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(controlImage);
        }

        // GET: ControlImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controlImage = await _context.ControlImage.FindAsync(id);
            if (controlImage == null)
            {
                return NotFound();
            }
            return View(controlImage);
        }

        // POST: ControlImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ControlImageId,Title,Label,Path")] ControlImage controlImage)
        {
            if (id != controlImage.ControlImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(controlImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ControlImageExists(controlImage.ControlImageId))
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
            return View(controlImage);
        }

        // GET: ControlImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controlImage = await _context.ControlImage
                .FirstOrDefaultAsync(m => m.ControlImageId == id);
            if (controlImage == null)
            {
                return NotFound();
            }

            return View(controlImage);
        }

        // POST: ControlImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var controlImage = await _context.ControlImage.FindAsync(id);
            _context.ControlImage.Remove(controlImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ControlImageExists(int id)
        {
            return _context.ControlImage.Any(e => e.ControlImageId == id);
        }
    }
}

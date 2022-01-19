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
    public class CharacteristicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharacteristicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Characteristics
        public async Task<IActionResult> Index(long? id)
        {
            if(id != null) ViewBag.Product = _context.Product.Find(id);
            var applicationDbContext = _context.Characteristic.Where(c=>c.ProductId == id || id == null).Include(c => c.Product.Catalog.Portal).Include(c => c.Property);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Characteristics/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characteristic = await _context.Characteristic.Include(c => c.Product.Catalog.Portal).Include(c => c.Property).FirstOrDefaultAsync(m => m.CharacteristicId == id);
            if (characteristic == null)
            {
                return NotFound();
            }

            return View(characteristic);
        }

        // GET: Characteristics/Create
        public IActionResult Create(long? Id)
        {
            var product = _context.Product.Include(c => c.Catalog.Portal).FirstOrDefault(m => m.ProductId == Id);
            ViewBag.Product = product;
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Label");
            ViewData["PropertyId"] = new SelectList(_context.Property, "PropertyId", "Label");
            return View();
        }

        // POST: Characteristics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CharacteristicId,ProductId,PropertyId,TextValue,DigitValue")] Characteristic characteristic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(characteristic);
                await _context.SaveChangesAsync();
                return Redirect($"/Characteristics/Index/" + characteristic.ProductId);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Label", characteristic.ProductId);
            ViewData["PropertyId"] = new SelectList(_context.Property, "PropertyId", "Label", characteristic.PropertyId);
            return View(characteristic);
        }

        // GET: Characteristics/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characteristic = await _context.Characteristic.Include(c => c.Product.Catalog.Portal).FirstOrDefaultAsync(m => m.CharacteristicId == id);
            if (characteristic == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Label", characteristic.ProductId);
            ViewData["PropertyId"] = new SelectList(_context.Property, "PropertyId", "Label", characteristic.PropertyId);
            return View(characteristic);
        }

        // POST: Characteristics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CharacteristicId,ProductId,PropertyId,TextValue,DigitValue")] Characteristic characteristic)
        {
            if (id != characteristic.CharacteristicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(characteristic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacteristicExists(characteristic.CharacteristicId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/Characteristics/Index/" + characteristic.ProductId);
                return RedirectToAction("Index", new { id = characteristic.ProductId });
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Label", characteristic.ProductId);
            ViewData["PropertyId"] = new SelectList(_context.Property, "PropertyId", "Label", characteristic.PropertyId);
            return View(characteristic);
        }

        // GET: Characteristics/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characteristic = await _context.Characteristic.Include(c => c.Product.Catalog.Portal).Include(c => c.Property).FirstOrDefaultAsync(m => m.CharacteristicId == id);
            if (characteristic == null)
            {
                return NotFound();
            }

            return View(characteristic);
        }

        // POST: Characteristics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var characteristic = await _context.Characteristic.FindAsync(id);
            id = characteristic.ProductId;
            _context.Characteristic.Remove(characteristic);
            await _context.SaveChangesAsync();
            return Redirect($"/Characteristics/Index/" + id);
            return RedirectToAction(nameof(Index));
        }

        private bool CharacteristicExists(long id)
        {
            return _context.Characteristic.Any(e => e.CharacteristicId == id);
        }
    }
}

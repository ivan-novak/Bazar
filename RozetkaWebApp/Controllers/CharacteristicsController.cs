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
        private readonly RozetkadbContext _context;

        public CharacteristicsController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Characteristics
        [HttpGet("[controller]/Index")]
        public async Task<IActionResult> Index(int? id, string Filter = null, int page = 0, int pageSize = 20)
        {
            if (id != null) ViewBag.Product = _context.Products.Include(c => c.Catalog.Portal).FirstOrDefault(m => m.ProductId == id);
            var applicationDbContext = _context.Characteristics.Include(c => c.Property).Where(c=>c.ProductId == id || id == null);
            
            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = applicationDbContext.Where(x => Filter == null || x.Property.Label.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.Property.Label).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }

        // GET: Characteristics/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characteristic = await _context.Characteristics.Include(c => c.Product.Catalog.Portal).Include(c => c.Property).FirstOrDefaultAsync(m => m.CharacteristicId == id);
            if (characteristic == null)
            {
                return NotFound();
            }

            return View(characteristic);
        }

        // GET: Characteristics/Create





        public IActionResult Create(long? Id)
        {
            var product = _context.Products.Include(c => c.Catalog.Portal).FirstOrDefault(m => m.ProductId == Id);
            ViewBag.Product = product;
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Label");
            ViewData["PropertyId"] = new SelectList(_context.Properties.Where(i => i.CatalogId == product.CatalogId), "PropertyId", "Label");
            return View();
        }

        // POST: Characteristics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CharacteristicId,ProductId,PropertyId,Value,Dimension")] Characteristic characteristic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(characteristic);
                await _context.SaveChangesAsync();
                return Redirect($"/Characteristics/Index/" + characteristic.ProductId);
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Label", characteristic.ProductId);
            ViewData["PropertyId"] = new SelectList(_context.Properties.Where(i => i.CatalogId == characteristic.Product.CatalogId), "PropertyId", "Label", characteristic.PropertyId);
            return View(characteristic);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characteristic = await _context.Characteristics.Include(c => c.Product.Catalog.Portal).FirstOrDefaultAsync(m => m.CharacteristicId == id);
            //var characteristic = await _context.Characteristics.FindAsync(id);
            if (characteristic == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Label", characteristic.ProductId);
            ViewData["PropertyId"] = new SelectList(_context.Properties.Where(i=>i.CatalogId==characteristic.Product.CatalogId), "PropertyId", "Label", characteristic.PropertyId);
            return View(characteristic);
        }

        // POST: Characteristics1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CharacteristicId,ProductId,PropertyId,Value,Dimension")] Characteristic characteristic)
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
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Label", characteristic.ProductId);
            ViewData["PropertyId"] = new SelectList(_context.Properties.Where(i => i.CatalogId == characteristic.Product.CatalogId), "PropertyId", "Label", characteristic.PropertyId);
            return View(characteristic);
        }
     
        // GET: Characteristics/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characteristic = await _context.Characteristics.Include(c => c.Product.Catalog.Portal).Include(c => c.Property).FirstOrDefaultAsync(m => m.CharacteristicId == id);
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
            var characteristic = await _context.Characteristics.FindAsync(id);
            id = characteristic.ProductId;
            _context.Characteristics.Remove(characteristic);
            await _context.SaveChangesAsync();
            return Redirect($"/Characteristics/Index/" + id);
            return RedirectToAction(nameof(Index));
        }

        private bool CharacteristicExists(long id)
        {
            return _context.Characteristics.Any(e => e.CharacteristicId == id);
        }
    }
}

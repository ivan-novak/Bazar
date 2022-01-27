﻿using System;
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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.Catalog = _context.Catalog.Include(c => c.Portal).First(i => i.CatalogId == id);
            var applicationDbContext = _context.Product.Where(c => c.CatalogId == id || id == null).Include(c => c.Catalog);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        //[HttpGet("[controller]/getDetails")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(c => c.Catalog.Portal).FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create(int? Id)
        {
            var catalog = _context.Catalog.Include(c => c.Portal).FirstOrDefault(m => m.CatalogId == Id); 
            ViewBag.Catalog = catalog;
            ViewData["CatalogId"] = new SelectList(_context.Catalog.Where(i => i.PortalId == catalog.PortalId), "CatalogId", "Label");
   
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,CatalogId,Title,Label,Description,Attributes,Price,Quantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return Redirect($"/Products/Index/" + product.CatalogId);
            }
            ViewBag.Catalog = product.Catalog;
            ViewData["CatalogId"] = new SelectList(_context.Catalog.Where(i => i.PortalId == product.Catalog.PortalId), "CatalogId", "Label");
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(c => c.Catalog).Include(c => c.Catalog.Portal).FirstOrDefaultAsync(m => m.ProductId == id); 
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Catalog = _context.Catalog.Find(product.CatalogId);
            ViewData["CatalogId"] = new SelectList(_context.Catalog.Where(i => i.PortalId == product.Catalog.PortalId), "CatalogId", "Label");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ProductId,CatalogId,Title,Label,Description,Attributes,Price,Quantity")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/Products/Index/" + product.CatalogId);
            }
         //   ViewBag.Catalog = _context.Catalog.Find(product.CatalogId);
            ViewData["CatalogId"] = new SelectList(_context.Catalog.Where(i => i.PortalId == product.Catalog.PortalId), "CatalogId", "Label");
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Catalog.Portal)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var product = await _context.Product.FindAsync(id);
            var CatalogId = product.CatalogId;
            _context.Product.Remove(product);
              await _context.SaveChangesAsync();
            return Redirect($"/Products/Index/" + CatalogId);
        }

        private bool ProductExists(long id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }


        [HttpGet("[controller]/{id}/image/{name}")]
        public async Task<FileResult> Image(long? id, string name)
        {
            if (id == null || name == null) return null;
            var productImage = await _context.ProductImage.Where(p=> p.ProductId==id && p.Label==name).FirstOrDefaultAsync();
            if (productImage == null) return null;
            var image = await _context.Image.FirstOrDefaultAsync(m => m.ImageId == productImage.ImageId);
            if (image == null) return null;
            return image.ToStream();
        }
    }
}



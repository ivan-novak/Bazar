﻿using System;
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
    public class ProductImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductImages
        public async Task<IActionResult> Index(int? id)
        {
            if (id != null) ViewBag.Product = _context.Product.Include(c => c.Catalog.Portal).FirstOrDefault(m => m.ProductId == id);
            var applicationDbContext = _context.ProductImage.Where(c => c.ProductId == id || id == null).Include(c => c.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductImages/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImage
                .Include(p => p.Image)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductImageId == id);
            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        // GET: ProductImages/Create
        public IActionResult Create(long? Id)
        {
            var product = _context.Product.Include(c => c.Catalog.Portal).FirstOrDefault(m => m.ProductId == Id);
            ViewBag.Product = product;
            ViewData["ImageId"] = new SelectList(_context.Image, "ImageId", "ImageId");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId");
            return View();
        }

        // POST: ProductImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductImageId,ProductId,Title,Label,ImageId")] ProductImage productImage)
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
                    productImage.ImageId = image.ImageId;
                    ms.Close();
                    ms.Dispose();
                }

                _context.Add(productImage);
                await _context.SaveChangesAsync();
                return Redirect($"/ProductImages/Index/" + productImage.ProductId);
                // return RedirectToAction(nameof(Index));
            }
            ViewData["ImageId"] = new SelectList(_context.Image, "ImageId", "ImageId", productImage.ImageId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", productImage.ProductId);
            return View(productImage);
        }

        // GET: ProductImages/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();
            

            var productImage = await _context.ProductImage.Include(c => c.Product.Catalog.Portal).FirstOrDefaultAsync(m => m.ProductImageId == id);
            if (productImage == null) return NotFound();
            
            ViewData["ImageId"] = new SelectList(_context.Image, "ImageId", "ImageId", productImage.ImageId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", productImage.ProductId);
            return View(productImage);
        }

        // POST: ProductImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ProductImageId,ProductId,Title,Label,ImageId")] ProductImage productImage)
        {
            if (id != productImage.ProductImageId)return NotFound();
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                var image = _context.Image.Find(productImage.ImageId);
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
                    _context.Update(productImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductImageExists(productImage.ProductImageId)) return NotFound();                   
                    else throw;                   
                }
                return Redirect($"/ProductImages/Index/" + productImage.ProductImageId);
            }
            ViewData["ImageId"] = new SelectList(_context.Image, "ImageId", "ImageId", productImage.ImageId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", productImage.ProductId);
            return View(productImage);
        }

        // GET: ProductImages/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();
            var productImage = await _context.ProductImage
                .Include(p => p.Image)
                .Include(p => p.Product.Catalog.Portal)
                .FirstOrDefaultAsync(m => m.ProductImageId == id);                          
            if (productImage == null) return NotFound();
            return View(productImage);
        }

        // POST: ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var productImage = await _context.ProductImage.Where(m => m.ProductImageId == id).FirstAsync();
            _context.ProductImage.Remove(productImage);
            await _context.SaveChangesAsync();
            return Redirect($"/ProductImages/Index/" + productImage.ProductId);
        }

        private bool ProductImageExists(long id)
        {
            return _context.ProductImage.Any(e => e.ProductImageId == id);
        }
    }
}
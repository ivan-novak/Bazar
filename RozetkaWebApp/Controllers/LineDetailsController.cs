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
    public class LineDetailsController : Controller
    {
        private readonly RozetkadbContext _context;

        public LineDetailsController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: LineDetails
        public async Task<IActionResult> Index()
        {
            var rozetkadbContext = _context.LineDetails.Include(l => l.Order).Include(l => l.Product);
            return View(await rozetkadbContext.ToListAsync());
        }

        // GET: LineDetails/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineDetail = await _context.LineDetails
                .Include(l => l.Order)
                .Include(l => l.Product)
                .FirstOrDefaultAsync(m => m.OrderDatailId == id);
            if (lineDetail == null)
            {
                return NotFound();
            }

            return View(lineDetail);
        }

        // GET: LineDetails/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "DeliveryAddress");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Label");
            return View();
        }

        // POST: LineDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDatailId,OrderId,UserId,CartId,ProductId,Quantities,UnitCost,Status,ExtOrderDetailNbr,CreateDate")] LineDetail lineDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lineDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "DeliveryAddress", lineDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Label", lineDetail.ProductId);
            return View(lineDetail);
        }

        // GET: LineDetails/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineDetail = await _context.LineDetails.FindAsync(id);
            if (lineDetail == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "DeliveryAddress", lineDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Label", lineDetail.ProductId);
            return View(lineDetail);
        }

        // POST: LineDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("OrderDatailId,OrderId,UserId,CartId,ProductId,Quantities,UnitCost,Status,ExtOrderDetailNbr,CreateDate")] LineDetail lineDetail)
        {
            if (id != lineDetail.OrderDatailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lineDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineDetailExists(lineDetail.OrderDatailId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "DeliveryAddress", lineDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Label", lineDetail.ProductId);
            return View(lineDetail);
        }

        // GET: LineDetails/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineDetail = await _context.LineDetails
                .Include(l => l.Order)
                .Include(l => l.Product)
                .FirstOrDefaultAsync(m => m.OrderDatailId == id);
            if (lineDetail == null)
            {
                return NotFound();
            }

            return View(lineDetail);
        }

        // POST: LineDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var lineDetail = await _context.LineDetails.FindAsync(id);
            _context.LineDetails.Remove(lineDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LineDetailExists(long id)
        {
            return _context.LineDetails.Any(e => e.OrderDatailId == id);
        }
    }
}
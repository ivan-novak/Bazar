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
    public class CommentsController : Controller
    {
        private readonly RozetkadbContext _context;

        public CommentsController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index(long? id, string Filter = null, int page = 0, int pageSize = 20)
        {
            ViewBag.CommentTab = "Active";
            if (id != null) ViewBag.Product = _context.Products.Include(c => c.Catalog.Portal).FirstOrDefault(m => m.ProductId == id);

            var rozetkadbContext = _context.Comments.Include(c => c.Image).Include(c => c.Product).Include(c => c.User).Where(c => c.ProductId == id);

            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = rozetkadbContext.Where(x => Filter == null || x.Text.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderByDescending(x => x.Date).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Image)
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Label");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,UserId,ProductId,Text,Date,Score,ImageId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId", comment.ImageId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Label", comment.ProductId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id == null)
            {
                return NotFound();
            }
            var characteristic = await _context.Characteristics.Include(c => c.Product.Catalog.Portal).FirstOrDefaultAsync(m => m.CharacteristicId == id);
            var comment = await _context.Comments.Include( x => x.Product).Include( x=> x.Product.Catalog).Include(x => x.Product.Catalog.Portal).FirstOrDefaultAsync(x => x.CommentId == id);


            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId", comment.ImageId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Label", comment.ProductId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", comment.UserId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CommentId,UserId,ProductId,Text,Date,Score,ImageId")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
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
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId", comment.ImageId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Label", comment.ProductId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.Include(c => c.Product.Catalog.Portal).FirstOrDefaultAsync(m => m.CommentId == id);


            //var comment = await _context.Comments
            //    .Include(c => c.Image)
            //    .Include(c => c.Product)
            //    .Include(c => c.User)
            //    .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(long id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}

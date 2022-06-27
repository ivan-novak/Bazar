using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;


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

        public async Task<IActionResult> MyComment(string id, string Filter = null, int page = 0, int pageSize = 20)
        {
            if (id != null) ViewBag.User = _context.AspNetUsers.Find(id);

            var rozetkadbContext = _context.Comments.Include(c => c.Image)
                .Include(c => c.Product).Include(c => c.User).Where(c => c.UserId == id);

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
        public IActionResult Create(long? Id)
        {
            ViewBag.Product = _context.Products.Include(c => c.Catalog.Portal).FirstOrDefault(m => m.ProductId == Id);
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,UserId,ProductId,Pros,Cons,Text,Date,Score,ImageId")] Comment comment)
        {
            if (ModelState.IsValid && comment.Text != null)
            {
                comment.Date = DateTime.Now;
                comment.UserId = _context.AspNetUsers.Where(x => x.Email == User.Identity.Name).First().Id;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return Redirect($"/Comments/Index/" + comment.ProductId);
            }
            ViewBag.Product = _context.Products.Include(c => c.Catalog.Portal).FirstOrDefault(m => m.ProductId == comment.ProductId);
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
            var comment = await _context.Comments.Include(x => x.Product).Include(x => x.Product.Catalog).Include(x => x.Product.Catalog.Portal).FirstOrDefaultAsync(x => x.CommentId == id);

            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CommentId,UserId,ProductId,Pros,Cons,Text,Date,Score,ImageId")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    comment.Date = DateTime.Now;
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
                return Redirect($"/Comments/Index/" + comment.ProductId);
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.Include(c => c.Product.Catalog.Portal).
                Include(c => c.User).FirstOrDefaultAsync(m => m.CommentId == id);


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
     //   [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id, string returnUrl = null)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            if (returnUrl != null) return Redirect(returnUrl);

            return Redirect($"/Comments/Index/" + comment.ProductId);
        }

        private bool CommentExists(long id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}

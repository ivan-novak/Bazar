//MLHIDEFILE
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RozetkaWebApp.Controllers
{
    [Authorize(Roles = "Користувачі")]
    public class PromotionsController : Controller
    {
        private readonly RozetkadbContext _context;

        public PromotionsController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Promotions
        public async Task<IActionResult> Index(string Filter = null, int page = 0, int pageSize = 20)
        {

            var applicationDbContext = _context.Promotions.Include(p => p.Image);
            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = applicationDbContext.Where(x => Filter == null || x.Label.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.Label).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }


        // GET: Promotions/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions
                .Include(p => p.Image)
                .FirstOrDefaultAsync(m => m.PromotionId == id);
            if (promotion == null)
            {
                return NotFound();
            }
            ViewBag.Lines = _context.Products.Where(x => x.PromotionId == id).Select(x => x);
            return View(promotion);
        }
        [Authorize(Roles = "Маркетологи")]

        // GET: Promotions/Create
        public IActionResult Create()
        {
            ViewBag.ImageId = new SelectList(_context.Images, "ImageId", "ImageId");
            return View();
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: Promotions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PromotionId,Title,Label,Description,Attributes,StartDate,EndDate,ImageId")] Promotion promotion)
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
                    promotion.ImageId = image.ImageId;
                    ms.Close();
                    ms.Dispose();
                }
                _context.Add(promotion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ImageId = new SelectList(_context.Images, "ImageId", "ImageId", promotion.ImageId);
            return View(promotion);
        }
        [Authorize(Roles = "Маркетологи")]


        // GET: Promotions/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }
            ViewBag.ImageId = new SelectList(_context.Images, "ImageId", "ImageId", promotion.ImageId);
            return View(promotion);
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: Promotions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("PromotionId,Title,Label,Description,Attributes,StartDate,EndDate,ImageId")] Promotion promotion, string returnUrl = null)
        {
            if (id != promotion.PromotionId)
            {
                return NotFound();
            }
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                var image = _context.Images.Find(promotion.ImageId);
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
                    _context.Update(promotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionExists(promotion.PromotionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (returnUrl != null) return Redirect(returnUrl);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ImageId = new SelectList(_context.Images, "ImageId", "ImageId", promotion.ImageId);
            return View(promotion);
        }
        [Authorize(Roles = "Маркетологи")]

        // GET: Promotions/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions
                .Include(p => p.Image)
                .FirstOrDefaultAsync(m => m.PromotionId == id);
            if (promotion == null)
            {
                return NotFound();
            }
            ViewBag.Lines = _context.Products.Where(x => x.PromotionId == id).Select(x => x);
            return View(promotion);
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: Promotions/Delete/5
        [HttpPost, ActionName("Delete")]
      //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromotionExists(long id)
        {
            return _context.Promotions.Any(e => e.PromotionId == id);
        }
    }
}

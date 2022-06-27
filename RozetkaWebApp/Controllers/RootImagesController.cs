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
    public class RootImagesController : Controller
    {
        private readonly RozetkadbContext _context;

        public RootImagesController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: RootImages
        public async Task<IActionResult> Index(string Filter = null, int page = 0, int pageSize = 20)
        {
            var applicationDbContext = _context.RootImages.Include(r => r.Image);

            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = applicationDbContext.Where(x => Filter == null || x.Label.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.Label).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }

        // GET: RootImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rootImage = await _context.RootImages
                .Include(r => r.Image)
                .FirstOrDefaultAsync(m => m.RootImageId == id);
            if (rootImage == null)
            {
                return NotFound();
            }

            return View(rootImage);
        }
        [Authorize(Roles = "Маркетологи")]

        // GET: RootImages/Create
        public IActionResult Create()
        {
            ViewBag.ImageId = new SelectList(_context.Images, "ImageId", "ImageId");
            return View();
        }

        // POST: RootImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RootImageId,Title,Label,ImageId")] RootImage rootImage)
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
                    rootImage.ImageId = image.ImageId;
                    ms.Close();
                    ms.Dispose();
                }


                _context.Add(rootImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ImageId = new SelectList(_context.Images, "ImageId", "ImageId", rootImage.ImageId);
            return View(rootImage);
        }
        [Authorize(Roles = "Маркетологи")]

        // GET: RootImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rootImage = await _context.RootImages.FindAsync(id);
            if (rootImage == null)
            {
                return NotFound();
            }
            ViewBag.ImageId = new SelectList(_context.Images, "ImageId", "ImageId", rootImage.ImageId);
            return View(rootImage);
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: RootImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RootImageId,Title,Label,ImageId")] RootImage rootImage)
        {
            if (id != rootImage.RootImageId)
            {
                return NotFound();
            }

            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                var image = _context.Images.Find(rootImage.ImageId);
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
                    _context.Update(rootImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RootImageExists(rootImage.RootImageId))
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
            ViewBag.ImageId = new SelectList(_context.Images, "ImageId", "ImageId", rootImage.ImageId);
            return View(rootImage);
        }
        [Authorize(Roles = "Маркетологи")]

        // GET: RootImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rootImage = await _context.RootImages
                .Include(r => r.Image)
                .FirstOrDefaultAsync(m => m.RootImageId == id);
            if (rootImage == null)
            {
                return NotFound();
            }

            return View(rootImage);
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: RootImages/Delete/5
        [HttpPost, ActionName("Delete")]
     //   [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string returnUrl = null)
        {
            var rootImage = await _context.RootImages.Where(m => m.RootImageId == id).FirstAsync();
            _context.RootImages.Remove(rootImage);
            await _context.SaveChangesAsync();
            if (returnUrl != null) return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }

        private bool RootImageExists(int id)
        {
            return _context.RootImages.Any(e => e.RootImageId == id);
        }

        //[HttpGet("[controller]/image/{name}")]
        //public async Task<FileResult> Image(long? id, string name)
        //{
        //    if (id == null || name == null) return null;
        //    var rootImage = await _context.RootImages.Where(p =>  p.Label == name).FirstOrDefaultAsync();
        //    if (rootImage == null) return null;
        //    var image = await _context.Images.FirstOrDefaultAsync(m => m.ImageId == rootImages.ImageId);
        //    if (image == null) return null;
        //    return Images.ToStream();
        //}
    }
}

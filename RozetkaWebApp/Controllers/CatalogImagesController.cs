//MLHIDEFILE
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RozetkaWebApp.Controllers
{
    [Authorize(Roles = "Користувачі")]
    public class CatalogImagesController : Controller
    {
        private readonly RozetkadbContext _context;

        public CatalogImagesController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: CatalogImages
        public async Task<IActionResult> Index(int? id, string Filter = null, int page = 0, int pageSize = 20)
        {
            if (id != null) ViewBag.Catalog = _context.Catalogs.Where(i => i.CatalogId == id).Include(c => c.Portal).First();
            var applicationDbContext = _context.CatalogImages.Where(c => c.CatalogId == id || id == null).Include(c => c.Catalog);
            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = applicationDbContext.Where(x => Filter == null || x.Label.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.Label).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }


        // GET: CatalogImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var catalogImage = await _context.CatalogImages.Where(m => m.Id == id).Include(c => c.Catalog).FirstAsync();
            if (catalogImage == null) return NotFound();
            return View(catalogImage);
        }

        [Authorize(Roles = "Маркетологи")]
        // GET: CatalogImages/Create
        public IActionResult Create(int? Id)
        {
            var catalog = _context.Catalogs.Where(m => m.CatalogId == Id).Include(c => c.Portal).First();
            ViewBag.Catalog = catalog;
            return View();
        }

        [Authorize(Roles = "Маркетологи")]

        // POST: CatalogImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatalogId,Title,Label,Data")] CatalogImage catalogImage)
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
                    catalogImage.ImageId = image.ImageId;
                    ms.Close();
                    ms.Dispose();
                }
                _context.Add(catalogImage);
                await _context.SaveChangesAsync();
                return Redirect($"/CatalogImages/Index/" + catalogImage.CatalogId);
            }
            return View(catalogImage);
        }

        [Authorize(Roles = "Маркетологи")]

        // GET: CatalogImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            CatalogImage catalogImage = await _context.CatalogImages.FindAsync(id);
            if (catalogImage == null) return NotFound();
            ViewBag.Catalog = _context.Catalogs.Where(i => i.CatalogId == catalogImage.CatalogId).Include(i => i.Portal).First();
            return View(catalogImage);
        }


        [Authorize(Roles = "Маркетологи")]

        // POST: CatalogImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CatalogId,ImageId,Title,Label,Data")] CatalogImage catalogImage)
        {
            if (id != catalogImage.Id) return NotFound();
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                var image = _context.Images.Find(catalogImage.ImageId);
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
                    _context.Update(catalogImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatalogImageExists(catalogImage.Id)) return NotFound();
                    else throw;
                }
                return Redirect($"/CatalogImages/Index/" + catalogImage.CatalogId);
            }
            return View(catalogImage);
        }


        [Authorize(Roles = "Маркетологи")]

        // GET: CatalogImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var catalogImage = await _context.CatalogImages.Where(m => m.Id == id).FirstAsync();
            if (catalogImage == null) return NotFound();
            ViewBag.Catalog = _context.Catalogs.Where(i => i.CatalogId == catalogImage.CatalogId).Include(c => c.Portal).First();
            return View(catalogImage);
        }


        [Authorize(Roles = "Маркетологи")]
        // POST: CatalogImages/Delete/5
        [HttpPost, ActionName("Delete")]
      //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string returnUrl = null)
        {
            var catalogImage = await _context.CatalogImages.FindAsync(id);
            _context.CatalogImages.Remove(catalogImage);
            await _context.SaveChangesAsync();
            if (returnUrl != null) return Redirect(returnUrl);
            return Redirect($"/CatalogImages/Index/" + catalogImage.CatalogId);
        }

        private bool CatalogImageExists(int id)
        {
            return _context.CatalogImages.Any(e => e.Id == id);
        }
    }
}

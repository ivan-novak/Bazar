using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RozetkaWebApp
{
    [Authorize(Roles = "Користувачі")]

    public class CatalogsController : Controller
    {
        private readonly RozetkadbContext _context;

        public CatalogsController(RozetkadbContext context)
        {
            _context = context;
        }

        public RedirectResult RedirectBack(string Url)
        {
            return Redirect(Url);
            if (ViewBag.returnUrl == null) return Redirect(Url);
            return Redirect(ViewBag.returnUrl);
        }
        public async Task<IActionResult> Index(int? id, string Filter = null, int page = 0, int pageSize = 20)
        {
            ViewBag.Portal = _context.Portals.Find(id);
            var applicationDbContext = _context.Catalogs.Where(c => c.PortalId == id || id == null).Include(c => c.Portal);
            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = applicationDbContext.Where(x => Filter == null || x.Label.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.Label).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }

        public async Task<FileResult> Image(int? id)
        {
            if (id == null) return null;
            var image = await _context.CatalogImages.FirstOrDefaultAsync(m => m.Id == id);
            if (image == null) return null;
            System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(image.Data);
            return new FileStreamResult(oMemoryStream, "image/*");
        }

        public async Task<IActionResult> Details(int? id, string Filter = null, int page = 0, int pageSize = 20)
        {
            if (id == null) return NotFound();
            var catalog = await _context.Catalogs
                .Include(c => c.Portal)
                .FirstOrDefaultAsync(c => c.CatalogId == id);
            if (catalog == null) return NotFound();
            ViewBag.Filter = Filter;
            ViewBag.Selectors = _context.Filters.Where(c=>c.CatalogId == id).ToList();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Portal = catalog.Portal;
            var products = _context.Products.
                Where(x => x.CatalogId == id).Where(x => Filter == null || x.Label.Contains(Filter))
                .OrderBy(x => x.Label).Skip(pageSize * page).Take(pageSize);
            ViewBag.TotalCount = products.Count();
            ViewBag.Products = products.ToList();
            ViewBag.Advertising = _context.Products.OrderByDescending(x => x.ChoiceCount).Take(6).ToList();
            return View(catalog);
        }

        [Authorize(Roles = "Маркетологи")]
        public IActionResult Create(int? Id)
        {
            var catalog = _context.Portals.FirstOrDefault(m => m.PortalId == Id);
            ViewBag.Portal = catalog;
            ViewBag.PortalId = new SelectList(_context.Portals, "PortalId", "Label");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatalogId,PortalId,Title,Label,Description,Attributes")] Catalog catalog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catalog);
                await _context.SaveChangesAsync();
                return RedirectBack($"/Catalogs/Index/" + catalog.PortalId);
            }
            ViewBag.PortalId = new SelectList(_context.Portals, "PortalId", "Label", catalog.PortalId);
            return View(catalog);
        }

        [Authorize(Roles = "Маркетологи")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var catalog = await _context.Catalogs
                .Include(c => c.Portal)
                .FirstOrDefaultAsync(m => m.CatalogId == id);
            if (catalog == null) return NotFound();
            ViewBag.PortalId = new SelectList(_context.Portals, "PortalId", "Label", catalog.PortalId);
            return View(catalog);
        }

        [Authorize(Roles = "Маркетологи")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatalogId,PortalId,Title,Label,Description,Attributes")] Catalog catalog)
        {
            if (id != catalog.CatalogId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catalog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatalogExists(catalog.CatalogId)) return NotFound();
                    else throw;
                }
                return RedirectBack($"/Catalogs/Index/" + catalog.PortalId);
            }
            ViewBag.PortalId = new SelectList(_context.Portals, "PortalId", "Label", catalog.PortalId);
            return View(catalog);
        }


        [Authorize(Roles = "Маркетологи")]
        public async Task<IActionResult> Delete(int? id, string Filter = null, int page = 0, int pageSize = 20)
        {
            if (id == null) return NotFound();
            var catalog = await _context.Catalogs
                .Include(c => c.Portal)
                .FirstOrDefaultAsync(m => m.CatalogId == id);
            if (catalog == null) return NotFound();
            ViewBag.Selectors = _context.Filters.Where(c => c.CatalogId == id).ToList();
            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Portal = catalog.Portal;
            var products = _context.Products.
                Where(x => x.CatalogId == id).Where(x => Filter == null || x.Label.Contains(Filter))
                .OrderBy(x => x.Label).Skip(pageSize * page).Take(pageSize);
            ViewBag.TotalCount = products.Count();
            ViewBag.Products = products.ToList();
            ViewBag.Advertising = _context.Products.OrderByDescending(x => x.ChoiceCount).Take(6).ToList();
            return View(catalog);
        }


        [Authorize(Roles = "Маркетологи")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catalog = await _context.Catalogs.FindAsync(id);
            var Id = catalog.PortalId;
            _context.Catalogs.Remove(catalog);
            await _context.SaveChangesAsync();
            return RedirectBack($"/Catalogs/Details/" + Id);
        }

        private bool CatalogExists(int id)
        {
            return _context.Catalogs.Any(e => e.CatalogId == id);
        }

    }
}

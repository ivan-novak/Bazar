using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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

        public async Task<IActionResult> Details(int? id, string Filter = null, int page = 0, int pageSize = 20, string chioces = null, string orderBy = null)
        {
            if (id == null) return NotFound();
            var catalog = await _context.Catalogs
                .Include(c => c.Portal)
                .FirstOrDefaultAsync(c => c.CatalogId == id);
            if (catalog == null) return NotFound();
            ViewBag.Filter = Filter;
            ViewBag.Selectors = _context.Filters.Where(c => c.CatalogId == id).ToList();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Chioces = chioces;
            ViewBag.Portal = catalog.Portal;
            ViewBag.OrderBy = orderBy;
            var query = _context.Products.Include(x => x.Comments).Where(x => x.CatalogId == id).Where(x => Filter == null || x.Label.Contains(Filter));
            if (chioces != null)
            {
                var chiocesList = chioces.Split('|').Select(x => new { group = x.Split("=") }).Where(x => x.group.Length == 2);
                var filterSet = _context.Characteristics.Select(x => new
                {
                    ProductId = x.ProductId,
                    PropertyId = x.PropertyId,
                    Value = (x.Value+ x.Dimension).Replace("+","").Replace("&", "").Replace("?", "").Replace("=", "").Replace(" ","")
                });
                foreach (var propertyId in chiocesList.Select(x => x.group[0]).Distinct())
                {
                    var valueSet = chiocesList.Where(c => c.group[0] == propertyId).Select(x => x.group[1]);
                    var productSet = filterSet.Where(x => (x.PropertyId.ToString() == propertyId && valueSet.Contains(x.Value))).Select(x => x.ProductId);
                    query = query.Where(x => productSet.Contains(x.ProductId));
                }
            }
            if (orderBy?.ToUpper() == "PRICEDOWN")
                query = query.OrderBy(x => x.Price);
            else
                if (orderBy?.ToUpper() == "PRICEUP")
                query = query.OrderByDescending(x => x.Price);
            else
                if (orderBy?.ToUpper() == "RATING")
                query = query.OrderByDescending(x => x.Comments.Average(x=>x.Score));
            else
                if (orderBy?.ToUpper() == "POPULARITY")
                query = query.OrderByDescending(x => x.ViewCount);
            else
                 if (orderBy?.ToUpper() == "NOVILTY")
                query = query.OrderByDescending(x => x.ProductId);
            else
                 if (orderBy?.ToUpper() == "PROMOTION")
                query = query.OrderBy(x => x.Label).OrderByDescending(x => x.Promotion);
            else
                query = query.OrderBy(x => x.Label);
            var products = query.Skip(pageSize * page).Take(pageSize);
            ViewBag.TotalCount = products.Count();
            ViewBag.Products = products.ToList();
            ViewBag.Advertising = _context.Products.Include(x=> x.Comments).OrderByDescending(x => x.ChoiceCount).Take(6).ToList();
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
        public async Task<IActionResult> Create([Bind("CatalogId,PortalId,Title,Label,Description,Attributes")] Catalog catalog,string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catalog);
                await _context.SaveChangesAsync();
                if (returnUrl != null) return Redirect(returnUrl);
                return Redirect($"/Catalogs/Index/" + catalog.PortalId);
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
        public async Task<IActionResult> Edit(int id, [Bind("CatalogId,PortalId,Title,Label,Description,Attributes")] Catalog catalog, string returnUrl = null)
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
                if (returnUrl != null) return Redirect(returnUrl);
                return Redirect($"/Catalogs/Index/" + catalog.PortalId);
            }
            ViewBag.PortalId = new SelectList(_context.Portals, "PortalId", "Label", catalog.PortalId);
            return View(catalog);
        }


        [Authorize(Roles = "Маркетологи")]
        public async Task<IActionResult> Delete(int? id, string Filter = null, int page = 0, int pageSize = 20, string chioces = null)
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
            ViewBag.Chioces = chioces;
            var query = _context.Products.Where(x => x.CatalogId == id).Where(x => Filter == null || x.Label.Contains(Filter));
            if (chioces != null)
            {
                var chiocesList = chioces.Split('|').Select(x => new { group = x.Split("=") }).Where(x => x.group.Length == 2);
                var filterSet = _context.Characteristics.Select(x => new
                {
                    ProductId = x.ProductId,
                    PropertyId = x.PropertyId,
                    Value = (x.Value + x.Dimension).Replace("+", "").Replace("&", "").Replace("?", "").Replace("=", "").Replace(" ", "")
                });
                foreach (var propertyId in chiocesList.Select(x => x.group[0]).Distinct())
                {
                    var valueSet = chiocesList.Where(c => c.group[0] == propertyId).Select(x => x.group[1]);
                    var productSet = filterSet.Where(x => (x.PropertyId.ToString() == propertyId && valueSet.Contains(x.Value))).Select(x => x.ProductId);
                    query = query.Where(x => productSet.Contains(x.ProductId));
                }
            }
            var products = query.OrderBy(x => x.Label).Skip(pageSize * page).Take(pageSize);
            ViewBag.TotalCount = products.Count();
            ViewBag.Products = products.ToList();
            ViewBag.Advertising = _context.Products.OrderByDescending(x => x.ChoiceCount).Take(6).ToList();
            return View(catalog);
        }


        [Authorize(Roles = "Маркетологи")]
        [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string returnUrl = null)
        {
            var catalog = await _context.Catalogs.FindAsync(id);
            var Id = catalog.PortalId;
            _context.Catalogs.Remove(catalog);
            await _context.SaveChangesAsync();
            if (returnUrl != null) return Redirect(returnUrl);
            return Redirect($"/Catalogs/Index/" + catalog.PortalId);
        }

        private bool CatalogExists(int id)
        {
            return _context.Catalogs.Any(e => e.CatalogId == id);
        }

    }
}

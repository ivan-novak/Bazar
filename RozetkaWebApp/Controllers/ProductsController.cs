//MLHIDEFILE
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RozetkaWebApp.Controllers
{
    [Authorize(Roles = "Користувачі")]

    public class ProductsController : Controller
    {
        private readonly RozetkadbContext _context;

        public ProductsController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? id, string Filter = null, int page = 0, int pageSize = 20)
        {
            ViewBag.Catalog = _context.Catalogs.Include(c => c.Portal).First(i => i.CatalogId == id);
            var applicationDbContext = _context.Products.Where(c => c.CatalogId == id || id == null).Include(c => c.Catalog);

            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = applicationDbContext.Where(x => Filter == null || x.Label.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.Label).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }

        public async Task<IActionResult> Audience(int? id, string Filter = null, int page = 0, int pageSize = 20)
        {
            ViewBag.Product = _context.Products.Where(c => c.ProductId == id).Include(c => c.Catalog).Include(c => c.Catalog.Portal).First();
            ViewBag.Catalog = ViewBag.Product.Catalog;
            var applicationDbContext = _context.Views.Where(c => c.ProductId == id).Where(c => c.UserId != null).Include(c => c.User);

            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = applicationDbContext.Where(x => Filter == null || x.User.UserName.Contains(Filter));
            var query1 = query.Select(x => x.User).Distinct();
            ViewBag.TotalCount = query1.Count();
            return View(await query1.OrderBy(x => x.UserName).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }

        // GET: Products/Details/5
        //[HttpGet("[controller]/getDetails")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products
                .Include(c => c.Catalog.Portal)
                .Include(p => p.Promotion)
                .Include(cm => cm.Comments)
                .ThenInclude(cs => cs.User)
                .Include(cs => cs.Characteristics)
                .ThenInclude(cs => cs.Property)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null) return NotFound();
            ViewBag.Advertising = _context.Products.OrderByDescending(x => x.ChoiceCount).Take(6).ToList();
            return View(product);
        }
        [Authorize(Roles = "Маркетологи")]

        // GET: Products/Create
        public IActionResult Create(int? Id)
        {
            var catalog = _context.Catalogs.Include(c => c.Portal).FirstOrDefault(m => m.CatalogId == Id);
            ViewBag.Catalog = catalog;
            ViewBag.CatalogId = new SelectList(_context.Catalogs.Where(i => i.PortalId == catalog.PortalId), "CatalogId", "Label");
            ViewBag.PromotionId = new SelectList(_context.Promotions.Where(i => i.EndDate >= DateTime.Now), "PromotionId", "Label");

            return View();
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,CatalogId,Title,Label,Description,Attributes,Price,Quantity,PromotionId,InventoryID")] Product product, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                if (returnUrl != null) return Redirect(returnUrl);
                return Redirect($"/Products/Index/" + product.CatalogId);
            }
            ViewBag.Catalog = product.Catalog;
            ViewBag.PromotionId = new SelectList(_context.Promotions.Where(i => i.EndDate >= DateTime.Now), "PromotionId", "Label");
            ViewBag.CatalogId = new SelectList(_context.Catalogs.Where(i => i.PortalId == product.Catalog.PortalId), "CatalogId", "Label");
            return View(product);
        }
        [Authorize(Roles = "Маркетологи")]

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(c => c.Catalog).Include(c => c.Catalog.Portal).FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Catalog = _context.Catalogs.Find(product.CatalogId);
            ViewBag.PromotionId = new SelectList(_context.Promotions.Where(i => i.EndDate >= DateTime.Now), "PromotionId", "Label");
            ViewBag.CatalogId = new SelectList(_context.Catalogs.Where(i => i.PortalId == product.Catalog.PortalId), "CatalogId", "Label");
            return View(product);
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ProductId,CatalogId,Title,Label,Description,Attributes,Price,Quantity,PromotionId,InventoryID")] Product product, string returnUrl = null)
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
                if (returnUrl != null) return Redirect(returnUrl);
                return Redirect($"/Products/Index/" + product.CatalogId);
            }
            //   ViewBag.Catalog = _context.Catalogs.Find(product.CatalogId);
            ViewBag.CatalogId = new SelectList(_context.Catalogs.Where(i => i.PortalId == product.Catalog.PortalId), "CatalogId", "Label");
            ViewBag.PromotionId = new SelectList(_context.Promotions.Where(i => i.EndDate >= DateTime.Now), "PromotionId", "Label");
            return View(product);
        }
        [Authorize(Roles = "Маркетологи")]

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(c => c.Catalog.Portal)
                .Include(p => p.Promotion)
                .Include(cm => cm.Comments)
                .ThenInclude(cs => cs.User)
                .Include(cs => cs.Characteristics)
                .ThenInclude(cs => cs.Property)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null) return NotFound();
            ViewBag.Advertising = _context.Products.OrderByDescending(x => x.ChoiceCount).Take(6).ToList();
            return View(product);
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
      //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id, string returnUrl = null)
        {
            var product = await _context.Products.FindAsync(id);
            var CatalogId = product.CatalogId;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            if (returnUrl != null) return Redirect(returnUrl);
            return Redirect($"/Products/Index/" + product.CatalogId);
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }


        public async Task<IActionResult> AddToCart(long? id)
        {

            var product = _context.Products.Find(id);
            var user_Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartId = CartId();
            var query = _context.LineDetails.Include(x => x.Product).Where(x => x.OrderId == null);
            if (user_Id != null) query = query.Where(x => x.UserId == user_Id);
            else query = query.Where(x => x.CartId == cartId);
            LineDetail lineDetail = query.Where(x => x.ProductId == id).FirstOrDefault();
            if (lineDetail == null)
            {
                lineDetail = new LineDetail();
                lineDetail.CartId = CartId();
                lineDetail.UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                lineDetail.ProductId = (long)id;
                lineDetail.Quantities = 1;
                lineDetail.UnitCost = product.Price;
                _context.Add(lineDetail);
            }
            else
            {
                lineDetail.Quantities++;
                _context.Update(lineDetail);
            }
            await _context.SaveChangesAsync();
            return Redirect(HttpContext.Request.Headers["Referer"]);
            return Redirect($"/Home/Characteristics/" + id.ToString());
        }

        public string CartId()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("cartId"))
            {
                var cartId = Guid.NewGuid().ToString();
                HttpContext.Response.Cookies.Append("cartId", cartId);
                return cartId;
            }
            return HttpContext.Request.Cookies["cartId"];
        }






        //[HttpGet("[controller]/{id}/image/{name}")]
        //public async Task<FileResult> Image(long? id, string name)
        //{
        //    if (id == null || name == null) return null;
        //    var productImage = await _context.ProductImages.Where(p=> p.ProductId==id && p.Label==name).FirstOrDefaultAsync();
        //    if (productImage == null) return null;
        //    var image = await _context.Images.FirstOrDefaultAsync(m => m.ImageId == productImages.ImageId);
        //    if (image == null) return null;
        //    return Images.ToStream();
        //}
    }
}



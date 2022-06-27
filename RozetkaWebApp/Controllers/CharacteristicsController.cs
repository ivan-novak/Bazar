//MLHIDEFILE
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RozetkaWebApp.Controllers
{
    [Authorize(Roles = "Користувачі")]

    public class CharacteristicsController : Controller
    {
        private readonly RozetkadbContext _context;

        public CharacteristicsController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Characteristics
        public async Task<IActionResult> Index(int? id, string Filter = null, int page = 0, int pageSize = 20)
        {
            if (id != null) ViewBag.Product = _context.Products.Include(c => c.Catalog.Portal).FirstOrDefault(m => m.ProductId == id);
            var applicationDbContext = _context.Characteristics.Include(c => c.Property).Where(c => c.ProductId == id || id == null);

            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = applicationDbContext.Where(x => Filter == null || x.Property.Label.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.Property.Label).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }

        // GET: Characteristics/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characteristic = await _context.Characteristics.Include(c => c.Product.Catalog.Portal).Include(c => c.Property).FirstOrDefaultAsync(m => m.CharacteristicId == id);
            if (characteristic == null)
            {
                return NotFound();
            }

            return View(characteristic);
        }

        // GET: Characteristics/Create




        [Authorize(Roles = "Маркетологи")]

        public IActionResult Create(long? Id)
        {
            var product = _context.Products.Include(c => c.Catalog.Portal).FirstOrDefault(m => m.ProductId == Id);
            ViewBag.Product = product;
            ViewBag.ProductId = new SelectList(_context.Products, "ProductId", "Label");
            ViewBag.PropertyId = new SelectList(_context.Properties.Where(i => i.CatalogId == product.CatalogId), "PropertyId", "Label");
            return View();
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: Characteristics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CharacteristicId,ProductId,PropertyId,Value,Dimension")] Characteristic characteristic, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                _context.Add(characteristic);
                await _context.SaveChangesAsync();
                if (returnUrl != null) return Redirect(returnUrl);
                return Redirect($"/Characteristics/Index/" + characteristic.ProductId.ToString());
            }
            ViewBag.ProductId = new SelectList(_context.Products, "ProductId", "Label", characteristic.ProductId);
            ViewBag.PropertyId = new SelectList(_context.Properties.Where(i => i.CatalogId == characteristic.Product.CatalogId), "PropertyId", "Label", characteristic.PropertyId);
            return View(characteristic);
        }
        [Authorize(Roles = "Маркетологи")]

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characteristic = await _context.Characteristics.Include(c => c.Product.Catalog.Portal).FirstOrDefaultAsync(m => m.CharacteristicId == id);
            //var characteristic = await _context.Characteristics.FindAsync(id);
            if (characteristic == null)
            {
                return NotFound();
            }
            ViewBag.ProductId = new SelectList(_context.Products, "ProductId", "Label", characteristic.ProductId);
            ViewBag.PropertyId = new SelectList(_context.Properties.Where(i => i.CatalogId == characteristic.Product.CatalogId), "PropertyId", "Label", characteristic.PropertyId);
            return View(characteristic);
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: Characteristics1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CharacteristicId,ProductId,PropertyId,Value,Dimension")] Characteristic characteristic, string returnUrl = null)
        {
            if (id != characteristic.CharacteristicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(characteristic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacteristicExists(characteristic.CharacteristicId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (returnUrl != null) return Redirect(returnUrl);
                return Redirect($"/Characteristics/Index/" + id);
            }
            ViewBag.ProductId = new SelectList(_context.Products, "ProductId", "Label", characteristic.ProductId);
            ViewBag.PropertyId = new SelectList(_context.Properties.Where(i => i.CatalogId == characteristic.Product.CatalogId), "PropertyId", "Label", characteristic.PropertyId);
            return View(characteristic);
        }
        [Authorize(Roles = "Маркетологи")]

        // GET: Characteristics/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characteristic = await _context.Characteristics.Include(c => c.Product.Catalog.Portal).Include(c => c.Property).FirstOrDefaultAsync(m => m.CharacteristicId == id);
            if (characteristic == null)
            {
                return NotFound();
            }

            return View(characteristic);
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: Characteristics/Delete/5
        [HttpPost, ActionName("Delete")]
     //   [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id, string returnUrl = null)
        {
            var characteristic = await _context.Characteristics.FindAsync(id);
            id = characteristic.ProductId;
            _context.Characteristics.Remove(characteristic);
            await _context.SaveChangesAsync();
            if (returnUrl != null) return Redirect(returnUrl);
            return Redirect($"/Characteristics/Index/" + id);
        }

        private bool CharacteristicExists(long id)
        {
            return _context.Characteristics.Any(e => e.CharacteristicId == id);
        }
    }
}

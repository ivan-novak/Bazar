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
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Portal(int? id)
        {
            var portal = _context.Portal
                .Include(p => p.Catalogs)
                .FirstOrDefault(p => p.PortalId == id);
            if (portal == null) return NotFound();
            return View(portal);
        }

        public IActionResult Catalog(int? id)
        {
            var catalog = _context.Catalog
                .Include(p => p.Products)
                .Include(p => p.Portal)
                .Include(p => p.Properties)
                .FirstOrDefault(p => p.CatalogId == id);
            if (catalog == null) return NotFound();
            return View(catalog);
        }

        public async Task<IActionResult> Product(int? id)
        {
            var product = await _context.Product
                .Where(p => p.ProductId == id)
                .Include(p => p.Catalog.Portal)
                .Include(p => p.Characteristics)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync();
            if (product == null) return NotFound();
            return View(product);
        }
    }
}

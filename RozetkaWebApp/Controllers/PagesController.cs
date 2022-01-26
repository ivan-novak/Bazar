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

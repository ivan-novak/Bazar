using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;

namespace RozetkaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Portal.ToListAsync());
        }


        public async Task<IActionResult> Catalogs(int? id)
        {
            return View(await _context.Catalog.Where(x=>x.PortalId ==id || id ==null).Include(x=> x.Portal).ToListAsync());
        }

        public async Task<IActionResult> Products(int? id)
        {
            return View(await _context.Product.Where(x => x.CatalogId == id || id == null).Include(x => x.Catalog).ToListAsync());
        }

        public async Task<IActionResult> Characteristics(int? id)
        {
            return View(await _context.Characteristic.Where(x => x.ProductId == id || id == null).Include(x => x.Product).Include(x => x.Property).ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }


}

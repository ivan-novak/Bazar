using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RozetkaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> Product(int? id)
        {
            var product = await _context.Product.Include(c => c.Catalog.Portal).FirstOrDefaultAsync(m => m.ProductId == id);

            //var product = _context.Product
            //    .Where(p => p.ProductId == id)
            //    .Include(p => p.Catalog)
            //    //.Include(p => p.Catalog.Portal)
            //    //.Include(p => p.Characteristics)
            //    //.Include(p => p.ProductImages)
            //    .FirstOrDefault();// (p => p.ProductId == id);
            if (product == null) return NotFound();
            return View(product);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}

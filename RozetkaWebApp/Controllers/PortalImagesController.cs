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

    public class PortalImagesController : Controller
    {
        private readonly RozetkadbContext _context;

        public PortalImagesController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: PortalImages
        public async Task<IActionResult> Index(int? id, string Filter = null, int page = 0, int pageSize = 20)
        {
            if (id != null) ViewBag.Portal = _context.Portals.Where(i => i.PortalId == id).First();
            var applicationDbContext = _context.PortalImages.Where(c => c.PortalId == id || id == null);

            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = applicationDbContext.Where(x => Filter == null || x.Label.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.Label).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }

        // GET: PortalImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portalImage = await _context.PortalImages
                .FirstOrDefaultAsync(m => m.PortalImageId == id);
            if (portalImage == null)
            {
                return NotFound();
            }

            return View(portalImage);
        }
        [Authorize(Roles = "Маркетологи")]

        // GET: PortalImages/Create
        public IActionResult Create(int? Id)
        {
            var portal = _context.Portals.Where(m => m.PortalId == Id).First();
            ViewBag.Portal = portal;
            return View();
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: PortalImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PortalImageId,PortalId,Title,Label,ImageId")] PortalImage portalImage)
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
                    portalImage.ImageId = image.ImageId;
                    ms.Close();
                    ms.Dispose();
                }
                _context.Add(portalImage);
                await _context.SaveChangesAsync();
                return Redirect($"/PortalImages/Index/" + portalImage.PortalId);
                // return RedirectToAction(nameof(Index));
            }
            return View(portalImage);
        }
        [Authorize(Roles = "Маркетологи")]

        // GET: PortalImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            PortalImage portalImage = await _context.PortalImages.FindAsync(id);
            if (portalImage == null) return NotFound();
            ViewBag.Portal = _context.Portals.Where(i => i.PortalId == portalImage.PortalId).First();
            return View(portalImage);
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: PortalImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PortalImageId,PortalId,Title,Label,ImageId")] PortalImage portalImage)
        {
            if (id != portalImage.PortalImageId)
            {
                return NotFound();
            }
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                var image = _context.Images.Find(portalImage.ImageId);
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
                    _context.Update(portalImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortalImageExists(portalImage.PortalImageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/PortalImages/Index/" + portalImage.PortalId);
            }
            return View(portalImage);
        }
        [Authorize(Roles = "Маркетологи")]

        // GET: PortalImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var portalImage = await _context.PortalImages.FirstOrDefaultAsync(m => m.PortalImageId == id);
            if (portalImage == null) return NotFound();
            ViewBag.Portal = _context.Portals.Where(i => i.PortalId == portalImage.PortalId).First();

            return View(portalImage);
        }
        [Authorize(Roles = "Маркетологи")]

        // POST: PortalImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var portalImage = await _context.PortalImages.FindAsync(id);
            _context.PortalImages.Remove(portalImage);
            await _context.SaveChangesAsync();
            return Redirect($"/PortalImages/Index/" + portalImage.PortalId);
        }

        private bool PortalImageExists(int id)
        {
            return _context.PortalImages.Any(e => e.PortalImageId == id);
        }

        //[HttpGet("[controller]/{id}/image/{name}")]
        //public async Task<FileResult> Image(long? id, string name)
        //{
        //    if (id == null || name == null) return null;
        //    var portalImage = await _context.PortalImages.Where(p => p.PortalId == id && p.Label == name).FirstOrDefaultAsync();
        //    if (portalImage == null) return null;
        //    var image = await _context.Images.FirstOrDefaultAsync(m => m.ImageId == portalImages.ImageId);
        //    if (image == null) return null;
        //  //  return Images.ToStream();
        //}
    }
}

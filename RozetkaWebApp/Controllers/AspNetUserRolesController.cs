//MLHIDEFILE
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RozetkaWebApp.Controllers
{
    [Authorize(Roles = "Користувачі")]
    public class AspNetUserRolesController : Controller
    {
        private readonly RozetkadbContext _context;

        public AspNetUserRolesController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: AspNetUserRoles
        public async Task<IActionResult> Index(string id, string Filter = null, int page = 0, int pageSize = 20)
        {
            if (id == null) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Role"] = _context.AspNetRoles.Find(id);
            var rozetkadbContext = _context.AspNetUserRoles.Include(a => a.User).Where(a => a.RoleId == id);

            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = rozetkadbContext.Where(x => Filter == null || x.User.UserName.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.User.UserName).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }

        // GET: AspNetUserRoles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var aspNetUserRole = await _context.AspNetUserRoles
                .Include(a => a.Role)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (aspNetUserRole == null)
            {
                return NotFound();
            }
            return View(aspNetUserRole);
        }

        [Authorize(Roles = "Адміністратори")]
        public IActionResult Create(string Id)
        {
            ViewData["Role"] = _context.AspNetRoles.Find(Id);
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            return View();
        }

        [Authorize(Roles = "Адміністратори")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId")] AspNetUserRole aspNetUserRole)
        {
            ViewData["Role"] = _context.AspNetRoles.Find(aspNetUserRole.RoleId);
            if (ModelState.IsValid)
            {
                _context.Add(aspNetUserRole);
                await _context.SaveChangesAsync();
                return Redirect($"/AspNetUserRoles/Index/" + aspNetUserRole.RoleId);
            }
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRole.UserId);
            return Redirect($"/AspNetUserRoles/Index/" + aspNetUserRole.RoleId);
        }

        [Authorize(Roles = "Адміністратори")]
        public async Task<IActionResult> Edit(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var aspNetUserRole = await _context.AspNetUserRoles.Where(a => a.UserId + a.RoleId == Id).FirstOrDefaultAsync();
            if (aspNetUserRole == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }

        [Authorize(Roles = "Адміністратори")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,RoleId")] AspNetUserRole aspNetUserRole)
        {
            if (id != aspNetUserRole.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUserRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserRoleExists(aspNetUserRole.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }

        [Authorize(Roles = "Адміністратори")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUserRole = await _context.AspNetUserRoles
                .Include(a => a.Role)
                .Include(a => a.User)
                .Where(a => a.UserId + a.RoleId == id).FirstOrDefaultAsync();
            if (aspNetUserRole == null)
            {
                return NotFound();
            }
            ViewData["Role"] = _context.AspNetRoles.Find(aspNetUserRole.RoleId);
            return View(aspNetUserRole);
        }

        [Authorize(Roles = "Адміністратори")]
        [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, string returnUrl = null)
        {
            var aspNetUserRole = await _context.AspNetUserRoles.Where(a => a.UserId + a.RoleId == id).FirstOrDefaultAsync();

            ViewData["Role"] = _context.AspNetRoles.Find(aspNetUserRole.RoleId);
            _context.AspNetUserRoles.Remove(aspNetUserRole);
            await _context.SaveChangesAsync();
            if (returnUrl != null) return Redirect(returnUrl);
            return Redirect($"/AspNetUserRoles/Index/" + aspNetUserRole.RoleId);
        }

        private bool AspNetUserRoleExists(string id)
        {
            return _context.AspNetUserRoles.Any(e => e.UserId == id);
        }
    }
}

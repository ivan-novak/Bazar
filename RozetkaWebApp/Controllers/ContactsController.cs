//MLHIDEFILE
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RozetkaWebApp.Controllers
{
    [Authorize(Roles = "Користувачі")]

    public class ContactsController : Controller
    {
        private readonly RozetkadbContext _context;

        public ContactsController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index(string id, string Filter = null, int page = 0, int pageSize = 20)
        {
            if (id == null) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.User = _context.AspNetUsers.Find(id);
            var rozetkadbContext = _context.Contacts.Include(a => a.User).Where(a => a.UserId == id);

            ViewBag.Filter = Filter;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var query = rozetkadbContext.Where(x => Filter == null || x.FullName.Contains(Filter));
            ViewBag.TotalCount = query.Count();
            return View(await query.OrderBy(x => x.FullName).Skip(pageSize * page).Take(pageSize).ToListAsync());
        }


        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewBag.User = _context.AspNetUsers.Find(contact.UserId);
            return View(contact);
        }
        [Authorize(Roles = "Продавці")]

        // GET: Contacts/Create
        public IActionResult Create(string id = null)
        {
            if (id == null) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.User = _context.AspNetUsers.Find(id);
            return View();
        }
        [Authorize(Roles = "Продавці")]

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,ContactType,FullName,DisplayName,Title,Salutation,Attention,FirstName,MidName,LastName,Email,WebSite,Fax,FaxType,Phone1,Phone1Type,Phone2,Phone2Type,Phone3,Phone3Type,DefAddressId,UserId,AssignDate,ExtAddressId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return Redirect($"/Contacts/Index/" + contact.UserId);
            }
            return Redirect($"/Contacts/Index/" + contact.UserId);

        }
        [Authorize(Roles = "Продавці")]

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewBag.User = _context.AspNetUsers.Find(contact.UserId);
            return View(contact);
        }
        [Authorize(Roles = "Продавці")]

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactId,ContactType,FullName,DisplayName,Title,Salutation,Attention,FirstName,MidName,LastName,Email,WebSite,Fax,FaxType,Phone1,Phone1Type,Phone2,Phone2Type,Phone3,Phone3Type,DefAddressId,UserId,AssignDate,ExtAddressId")] Contact contact, string returnUrl = null)
        {

            if (id != contact.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.ContactId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (returnUrl != null) return Redirect(returnUrl);
                return Redirect($"/Contacts/Index/" + contact.UserId);
            }
            return Redirect($"/Contacts/Index/" + contact.UserId);

        }
        [Authorize(Roles = "Продавці")]

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewBag.User = _context.AspNetUsers.Find(contact.UserId);
            return View(contact);
        }
        [Authorize(Roles = "Продавці")]

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
     //   [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string returnUrl = null)
        {
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            if (returnUrl != null) return Redirect(returnUrl);
            return Redirect($"/Contacts/Index/" + contact.UserId);
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RozetkaWebApp.Data;
using RozetkaWebApp.Models;

namespace RozetkaWebApp.Controllers
{
    public class ContactsController : Controller
    {
        private readonly RozetkadbContext _context;

        public ContactsController(RozetkadbContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
            var rozetkadbContext = _context.Contacts.Include(a => a.User).Where(a => a.UserId == userid);
            return View(await rozetkadbContext.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
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

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,ContactType,FullName,DisplayName,Title,Salutation,Attention,FirstName,MidName,LastName,Email,WebSite,Fax,FaxType,Phone1,Phone1Type,Phone2,Phone2Type,Phone3,Phone3Type,DefAddressId,UserId,AssignDate,ExtAddressId")] Contact contact)
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
            if (ModelState.IsValid)
            {
                contact.UserId = userid;
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", contact.UserId);
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", contact.UserId);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactId,ContactType,FullName,DisplayName,Title,Salutation,Attention,FirstName,MidName,LastName,Email,WebSite,Fax,FaxType,Phone1,Phone1Type,Phone2,Phone2Type,Phone3,Phone3Type,DefAddressId,UserId,AssignDate,ExtAddressId")] Contact contact)
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
            if (id != contact.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    contact.UserId = userid;
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", contact.UserId);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
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

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Redirect($"/Identity/Account/Register");
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactId == id);
        }
    }
}

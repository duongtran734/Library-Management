#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagment.Data;
using LibraryManagment.Models;
using LibraryManagment.Models.ViewModels;

namespace LibraryManagment.Controllers
{
    public class PatronsController : Controller
    {
        private readonly LibraryManagmentContext _context;

        public PatronsController(LibraryManagmentContext context)
        {
            _context = context;
        }

        // GET: Patrons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patron.ToListAsync());
        }

        // GET: Patrons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patron = await _context.Patron
                .Include(x => x.LibraryBranch)
                .Include(x=>x.LibraryCard)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patron == null)
            {
                return NotFound();
            }

            var model = new PatronDetailViewModel
            {
                Id = patron.Id,
                FirstName = patron.FirstName,
                LastName = patron.LastName,
                LibraryCardId = patron.LibraryCard.Id,
                Address = patron.Address,
                MemberSince = patron.LibraryCard.Created,
                OverdueFees = patron.LibraryCard.Fees,
                HomeBranchName = patron.LibraryBranch.Name,
                AssetsCheckedOut = GetCheckouts(patron.Id),
                CheckoutHistory = GetCheckoutHistory(patron.Id),
                Holds = GetHolds(patron.Id)
            };

            return View(model);
        }

        private IEnumerable<Hold> GetHolds(int id)
        {
            var patron = _context.Patron
              .Include(x => x.LibraryCard)
              .FirstOrDefault(x => x.Id == id);

            var holds = _context.Holds
              .Where(x => x.LibraryCard.Id == patron.LibraryCard.Id);

            return holds;
        }

        private IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            var patron = _context.Patron
                .Include(x => x.LibraryCard)
                .FirstOrDefault(x => x.Id == id);

            var histories = _context.CheckoutHistories
              .Where(x => x.LibraryCard.Id == patron.LibraryCard.Id);

            return histories;

        }

        private IEnumerable<Checkout> GetCheckouts(int id)
        {
            var patron = _context.Patron
                .Include(x=>x.LibraryCard)
                .FirstOrDefault(x => x.Id == id);

            var checkouts = _context.Checkouts
                .Include(x => x.LibraryAsset)
                .Where(x => x.LibraryCard.Id == patron.LibraryCard.Id);
            return checkouts;
        }

        // GET: Patrons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patrons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Address,DateOfBirth")] Patron patron)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patron);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patron);
        }

        // GET: Patrons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patron = await _context.Patron.FindAsync(id);
            if (patron == null)
            {
                return NotFound();
            }
            return View(patron);
        }

        // POST: Patrons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Address,DateOfBirth")] Patron patron)
        {
            if (id != patron.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patron);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatronExists(patron.Id))
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
            return View(patron);
        }

        // GET: Patrons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patron = await _context.Patron
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patron == null)
            {
                return NotFound();
            }

            return View(patron);
        }

        // POST: Patrons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patron = await _context.Patron.FindAsync(id);
            _context.Patron.Remove(patron);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatronExists(int id)
        {
            return _context.Patron.Any(e => e.Id == id);
        }
    }
}

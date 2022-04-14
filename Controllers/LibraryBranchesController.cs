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

namespace LibraryManagment.Controllers
{
    public class LibraryBranchesController : Controller
    {
        private readonly LibraryManagmentContext _context;

        public LibraryBranchesController(LibraryManagmentContext context)
        {
            _context = context;
        }

        // GET: LibraryBranches
        public async Task<IActionResult> Index()
        {
            return View(await _context.LibraryBranches.ToListAsync());
        }

        // GET: LibraryBranches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryBranch = await _context.LibraryBranches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryBranch == null)
            {
                return NotFound();
            }

            return View(libraryBranch);
        }

        // GET: LibraryBranches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LibraryBranches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Telephone,Description,OpenDate,ImageUrl")] LibraryBranch libraryBranch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libraryBranch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libraryBranch);
        }

        // GET: LibraryBranches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryBranch = await _context.LibraryBranches.FindAsync(id);
            if (libraryBranch == null)
            {
                return NotFound();
            }
            return View(libraryBranch);
        }

        // POST: LibraryBranches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Telephone,Description,OpenDate,ImageUrl")] LibraryBranch libraryBranch)
        {
            if (id != libraryBranch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libraryBranch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryBranchExists(libraryBranch.Id))
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
            return View(libraryBranch);
        }

        // GET: LibraryBranches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryBranch = await _context.LibraryBranches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryBranch == null)
            {
                return NotFound();
            }

            return View(libraryBranch);
        }

        // POST: LibraryBranches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libraryBranch = await _context.LibraryBranches.FindAsync(id);
            _context.LibraryBranches.Remove(libraryBranch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryBranchExists(int id)
        {
            return _context.LibraryBranches.Any(e => e.Id == id);
        }
    }
}

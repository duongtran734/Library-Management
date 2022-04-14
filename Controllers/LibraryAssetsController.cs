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
    public class LibraryAssetsController : Controller
    {
        private readonly LibraryManagmentContext _context;

        public LibraryAssetsController(LibraryManagmentContext context)
        {
            _context = context;
        }

        // GET: LibraryAssets
        public async Task<IActionResult> Index()
        {
            var assets = await _context.LibraryAssets
                .Include(asset => asset.Status)
                .Include(asset => asset.Location).ToListAsync();


            var listing = assets
                .Select(result => new AssetsIndexViewModel
                {
                    Id = result.Id,
                    ImgUrl = result.ImageUrl,
                    Title = result.Title,
                    AuthorOrDirector = GetAuthorOrDirector(result.Id),
                    DeweyCallNumber = GetDeweyIndex(result.Id),
                    Type = GetType(result.Id)
                });

            return View(listing);
        }

        // GET: LibraryAssets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryAsset = await _context.LibraryAssets
                .Include(_asset => _asset.Status)
                .Include(_asset => _asset.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryAsset == null)
            {
                return NotFound();
            }

            var currentHolds = GetCurrentHolds(id)
                .Select(a => new AssetHoldModel
                {
                    HoldPlaced = GetCurrentHoldPlaced(a.Id).ToString("d"),
                    PatronName = GetCurrentHoldPatronName(a.Id)
                });

            var model = new AssetsDetailViewModel
            {
                Id = libraryAsset.Id,
                ImgUrl = libraryAsset.ImageUrl,
                Title = libraryAsset.Title,
                AuthorOrDirector = GetAuthorOrDirector(libraryAsset.Id),
                Type = GetType(libraryAsset.Id),
                DeweyCallNumber = GetDeweyIndex(libraryAsset.Id),
                NumberOfCopies = libraryAsset.NumberOfCopies.ToString(),
                Year = libraryAsset.Year,
                ISBN = GetISBN(libraryAsset.Id),
                Status = libraryAsset.Status.Name,
                Cost = libraryAsset.Cost,
                CurrentLocation = libraryAsset.Location.Name,
                CheckoutHistories = GetCheckOutHistory(libraryAsset.Id),
                LastestCheckout = GetLatestCheckout(libraryAsset.Id),
                PatronName = GetCurrentCheckoutPatronName(libraryAsset.Id),
                CurrentHolds = currentHolds,
            };

            return View(model);
        }



        // GET: LibraryAssets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LibraryAssets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Year,Cost,NumberOfCopies,ImageUrl")] LibraryAsset libraryAsset)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libraryAsset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libraryAsset);
        }

        // GET: LibraryAssets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryAsset = await _context.LibraryAssets.FindAsync(id);
            if (libraryAsset == null)
            {
                return NotFound();
            }
            return View(libraryAsset);
        }

        // POST: LibraryAssets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Year,Cost,NumberOfCopies,ImageUrl")] LibraryAsset libraryAsset)
        {
            if (id != libraryAsset.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libraryAsset);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryAssetExists(libraryAsset.Id))
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
            return View(libraryAsset);
        }

        // GET: LibraryAssets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryAsset = await _context.LibraryAssets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryAsset == null)
            {
                return NotFound();
            }

            return View(libraryAsset);
        }

        // POST: LibraryAssets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libraryAsset = await _context.LibraryAssets.FindAsync(id);
            _context.LibraryAssets.Remove(libraryAsset);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryAssetExists(int id)
        {
            return _context.LibraryAssets.Any(e => e.Id == id);
        }

        /////////////////////////////Helper
        private string GetDeweyIndex(int id)
        {
            if (_context.Books.Any(book => book.Id == id))
            {
                return _context.Books.FirstOrDefault(book => book.Id == id).DeweyIndex;
            }
            return "";
        }

        private string GetISBN(int id)
        {
            if (_context.Books.Any(book => book.Id == id))
            {
                return _context.Books.FirstOrDefault(book => book.Id == id).ISBN;
            }
            return "";
        }

        private string GetAuthorOrDirector(int id)
        {
            var isBook = _context.LibraryAssets.OfType<Book>().Any(_book => _book.Id == id);
            return isBook ? _context.Books.FirstOrDefault(b => b.Id == id).Author :
                _context.Videos.FirstOrDefault(v => v.Id == id).Director
                ?? "Unkown id";
        }

        private string GetType(int id)
        {
            var isBook = _context.LibraryAssets.OfType<Book>().Any(_book => _book.Id == id);
            return isBook ? "Book" : "Video";
        }

        private IEnumerable<Hold> GetCurrentHolds(int? id)
        {
            return _context.Holds
                .Include(h => h.LibraryAsset)
                .Where(h => h.LibraryAsset.Id == id);
        }
        private DateTime GetCurrentHoldPlaced(int id)
        {
            //Find matching Id and return the date
            return _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h => h.Id == id)
                .HoldPlaced;
        }
        private string GetCurrentHoldPatronName(int id)
        {
            var hold = _context.Holds
                 .Include(h => h.LibraryAsset)
                 .Include(h => h.LibraryCard)
                 .FirstOrDefault(h => h.Id == id);

            var cardId = hold?.LibraryCard?.Id;
            var patron = _context.Patron
                .Include(p => p.LibraryCard)
                ?.FirstOrDefault(p => p.LibraryCard.Id == hold.LibraryCard.Id);

            return patron?.FirstName + " " + patron?.LastName;
        }

        private IEnumerable<CheckoutHistory> GetCheckOutHistory(int id)
        {
            return _context.CheckoutHistories
                .Include(h => h.LibraryAsset)
                .Include(h=>h.LibraryCard)
                .Where(h => h.LibraryAsset.Id == id);
        }

        private Checkout GetLatestCheckout(int id)
        {
            return _context.Checkouts
                .Where(c => c.LibraryAsset.Id == id)
                .OrderByDescending(c => c.Since)
                .FirstOrDefault();
        }

        private string GetCurrentCheckoutPatronName(int id)
        {
            var checkOut = _context.Checkouts
                .Include(c=>c.LibraryCard)
                .FirstOrDefault(c => c.LibraryAsset.Id == id);
            
            if (checkOut == null)
            {
                return "not checked out";
            }
            var patron = _context.Patron.FirstOrDefault(p => p.LibraryCard.Id == checkOut.LibraryCard.Id);
            return patron?.FirstName + " " + patron?.LastName;
        }

    }
}

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
    public class CheckoutsController : Controller
    {
        private readonly LibraryManagmentContext _context;

        public CheckoutsController(LibraryManagmentContext context)
        {
            _context = context;
        }

        // GET: Checkouts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Checkouts.ToListAsync());
        }

        // GET: Checkouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkout = await _context.Checkouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkout == null)
            {
                return NotFound();
            }

            return View(checkout);
        }

        // GET: Checkouts/Create
        public IActionResult CheckOut(int id)
        {
            var asset = _context.LibraryAssets.FirstOrDefault(m => m.Id == id);
            var model = new CheckoutModels
            {
                AssetId = id,
                ImgUlr = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = IsCheckedOut(id)
            };
            return View(model);
        }

        public IActionResult CheckIn(int assetId)
        {
            CheckInItem(assetId);
            return RedirectToAction("Details", "LibraryAssets", new { id = assetId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceCheckout(int assetId, int libraryCardId)
        {
            CheckOutItem(assetId, libraryCardId);
            return RedirectToAction("Details","LibraryAssets",new { id = assetId});
        }
        public IActionResult Hold(int assetId)
        {
            var asset = _context.LibraryAssets.FirstOrDefault(m => m.Id == assetId);
            var model = new CheckoutModels
            {
                AssetId = assetId,
                ImgUlr = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = IsCheckedOut(assetId),
                HoldCount = GetCurrentHolds(assetId).Count()
            };
            return View(model);
        }

   

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceHoldItem(int assetId, int libraryCardId)
        {
            PlaceHold(assetId, libraryCardId);
            return RedirectToAction("Details", "LibraryAssets", new { id = assetId });
        }


        public IActionResult MarkLostItem(int assetId)
        {
            MarkLost(assetId);
            return RedirectToAction("Details", "LibraryAssets", new { id = assetId });
        }
        public IActionResult MarkFoundItem(int assetId)
        {
            MarkFound(assetId);
            return RedirectToAction("Details", "LibraryAssets", new { id = assetId });
        }

        // GET: Checkouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkout = await _context.Checkouts.FindAsync(id);
            if (checkout == null)
            {
                return NotFound();
            }
            return View(checkout);
        }

        // POST: Checkouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Since,Until")] Checkout checkout)
        {
            if (id != checkout.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checkout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckoutExists(checkout.Id))
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
            return View(checkout);
        }

        // GET: Checkouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkout = await _context.Checkouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkout == null)
            {
                return NotFound();
            }

            return View(checkout);
        }

        // POST: Checkouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checkout = await _context.Checkouts.FindAsync(id);
            _context.Checkouts.Remove(checkout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckoutExists(int id)
        {
            return _context.Checkouts.Any(e => e.Id == id);
        }

        ///////////////////////////////////////
        private bool IsCheckedOut(int assetId)
        {
            return _context.Checkouts.Any(c => c.LibraryAsset.Id == assetId);
        }

        private void MarkLost(int assetId)
        {
            UpdateAssetStatus(assetId, "Lost");
            _context.SaveChanges();
        }
        private void MarkFound(int assetId)
        {
            var now = DateTime.Now;
            UpdateAssetStatus(assetId, "Available");
            RemoveExistingCheckouts(assetId);
            CloseExistingCheckoutHistory(assetId, now);



            _context.SaveChanges();
        }

        private void RemoveExistingCheckouts(int assetId)
        {
            //remove exisiting checkouts
            var checkout = _context.Checkouts.FirstOrDefault(a => a.LibraryAsset.Id == assetId);
            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }


        private void CloseExistingCheckoutHistory(int assetId, DateTime now)
        {
            // close any existing checkout history
            var history = _context.CheckoutHistories.FirstOrDefault(a => a.LibraryAsset.Id == assetId && a.CheckedIn == null);
            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = DateTime.Now;
            }
        }

        private void UpdateAssetStatus(int assetId, string status)
        {
            var item = _context.LibraryAssets.FirstOrDefault(a => a.Id == assetId);
            _context.Update(item);
            item.Status = _context.Statuses.FirstOrDefault(s => s.Name == status);
        }

        private void CheckInItem(int assetId)
        {
            var now = DateTime.Now;
            var item = _context.LibraryAssets.FirstOrDefault(a => a.Id == assetId);

            //Remove existing checkout
            RemoveExistingCheckouts(assetId);
            CloseExistingCheckoutHistory(assetId, now);
            var currentHolds = _context.Holds.Where(h => h.LibraryAsset.Id == assetId);
            if (currentHolds.Any())
            {
                CheckOutToEarliesHold(assetId, currentHolds);
                return;
            }
            UpdateAssetStatus(assetId, "Available");
            _context.SaveChanges();
        }

        private void CheckOutToEarliesHold(int assetId, IQueryable<Hold> currentHolds)
        {
            var earliestHold = currentHolds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .OrderBy(h=>h.HoldPlaced).FirstOrDefault();
            var card = earliestHold.LibraryCard;
            _context.Remove(earliestHold);
            _context.SaveChanges();
            CheckOutItem(assetId, card.Id);
        }

        private void CheckOutItem(int assetId, int cardId)
        {
            if (IsCheckedOut(assetId))
            {
                return;
            }
            var now = DateTime.Now;
            var item = _context.LibraryAssets.FirstOrDefault(a => a.Id == assetId);
            UpdateAssetStatus(assetId, "Checked Out");
            var libraryCard = _context.LibraryCards.FirstOrDefault(c => c.Id == cardId);
            var checkout = new Checkout
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultCheckoutTime(now)
            };

            _context.Add(checkout);

            var checkoutHistory = new CheckoutHistory
            {
                CheckedOut = now,
                LibraryAsset = item,
                LibraryCard = libraryCard
            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }

        private DateTime GetDefaultCheckoutTime(DateTime now)
        {
            return now.AddDays(30);
        }

        private void PlaceHold(int assetId, int cardId)
        {
            var now = DateTime.Now;
            var asset = _context.LibraryAssets
                .Include(s=>s.Status)
                .FirstOrDefault(a => a.Id == assetId);
            var card = _context.LibraryCards.FirstOrDefault(c => c.Id == cardId); 
            if(asset.Status.Name == "Available")
            {
                UpdateAssetStatus(assetId, "On Hold");
            }
            var hold = new Hold
            {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = card
            };
            _context.Add(hold);
            _context.SaveChanges();
        }

        private string GetCurrentHoldPatronName(int holdId)
        {
            var hold = _context.Holds.FirstOrDefault(c => c.Id == holdId);
            var cardId = hold?.LibraryCard?.Id;
            var patron = _context.Patron.Where(p=>p.LibraryCard.Id == cardId).FirstOrDefault();
            return patron?.FirstName + " " + patron?.LastName;
        }

        private DateTime GetCurrentHoldPlaced(int holdId)
        {
            return _context.Holds.FirstOrDefault(h => h.Id == holdId).HoldPlaced;

        }

        private string GetCurrentCheckoutPatronName(int id)
        {
            var checkOut = _context.Checkouts.FirstOrDefault(c => c.LibraryAsset.Id == id);

            if (checkOut == null)
            {
                return "not checked out";
            }
            var patron = _context.Patron.FirstOrDefault(p => p.LibraryCard.Id == checkOut.LibraryCard.Id);
            return patron.FirstName + " " + patron.LastName;
        }

        private IEnumerable<Hold> GetCurrentHolds(int assetId)
        {
            return _context.Holds.Where(h=>h.LibraryAsset.Id == assetId).ToList();
        }
    }
}

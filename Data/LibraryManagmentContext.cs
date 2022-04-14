#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryManagment.Models;

namespace LibraryManagment.Data
{
    public class LibraryManagmentContext : DbContext
    {
        public LibraryManagmentContext (DbContextOptions<LibraryManagmentContext> options)
            : base(options)
        {
        }

        public DbSet<LibraryManagment.Models.Book> Books { get; set; }
        public DbSet<LibraryManagment.Models.BranchHours> BranchHours { get; set; }
        public DbSet<LibraryManagment.Models.Checkout> Checkouts { get; set; }
        public DbSet<LibraryManagment.Models.CheckoutHistory> CheckoutHistories { get; set; }
        public DbSet<LibraryManagment.Models.Hold> Holds { get; set; }
        public DbSet<LibraryManagment.Models.LibraryAsset> LibraryAssets { get; set; }
        public DbSet<LibraryManagment.Models.LibraryBranch> LibraryBranches { get; set; }
        public DbSet<LibraryManagment.Models.LibraryCard> LibraryCards { get; set; }
        public DbSet<LibraryManagment.Models.Patron> Patron { get; set; }
        public DbSet<LibraryManagment.Models.Status> Statuses { get; set; }
        public DbSet<LibraryManagment.Models.Video> Videos { get; set; }
        
    }
}

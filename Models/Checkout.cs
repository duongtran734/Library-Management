using System.ComponentModel.DataAnnotations;

namespace LibraryManagment.Models
{
    public class Checkout
    {
        public int Id { get; set; }
        public DateTime Since { get; set; } // When Item was checked out
        public DateTime Until { get; set; } // When Item is due
        [Required]
        public LibraryAsset LibraryAsset { get; set; }
        [Required]
        public LibraryCard LibraryCard { get; set; }
        
    }
}

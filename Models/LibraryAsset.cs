using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagment.Models
{
    public abstract class LibraryAsset
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public int NumberOfCopies { get; set; }
        [DisplayName("Image")]
        public string ImageUrl { get; set; }
        public virtual LibraryBranch Location { get; set; }

    }
}
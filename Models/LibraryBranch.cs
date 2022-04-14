using System.ComponentModel.DataAnnotations;

namespace LibraryManagment.Models
{
    public class LibraryBranch
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OpenDate { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<Patron> Patrons { get; set; }
        public virtual ICollection<LibraryAsset> LibraryAssets{ get; set; }

    }
}
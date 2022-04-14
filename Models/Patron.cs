using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagment.Models
{
    public class Patron
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        public virtual LibraryCard LibraryCard { get; set; }
        public virtual LibraryBranch LibraryBranch { get; set; }


    }
}

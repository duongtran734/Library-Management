using System.ComponentModel.DataAnnotations;

namespace LibraryManagment.Models.ViewModels
{
    public class PatronDetailViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public int LibraryCardId { get; set; }
        public string Address { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime MemberSince { get; set; }
        public string HomeBranchName { get; set; }
        public decimal OverdueFees { get; set; }
        public IEnumerable<Checkout> AssetsCheckedOut  { get; set; }
        public IEnumerable<CheckoutHistory>  CheckoutHistory { get; set; }
        public IEnumerable<Hold> Holds { get; set; }
    }
}

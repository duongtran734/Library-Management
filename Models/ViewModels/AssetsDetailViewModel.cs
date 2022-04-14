namespace LibraryManagment.Models.ViewModels
{
    public class AssetsDetailViewModel : AssetsIndexViewModel
    {
        public int Year { get; set; }
        public string ISBN { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }
        public string CurrentLocation { get; set; }
        public string? PatronName { get; set; }
        public Checkout LastestCheckout { get; set; }
        public IEnumerable<CheckoutHistory> CheckoutHistories { get; set; }
        public IEnumerable<AssetHoldModel> CurrentHolds { get; set; }
    }

    public class AssetHoldModel
    {
        public string PatronName { get; set; }
        public string HoldPlaced { get; set; }
    }
}

namespace LibraryManagment.Models.ViewModels
{
    public class CheckoutModels
    {
        public string LibraryCardId { get; set; }
        public string Title { get; set; }
        public int AssetId { get; set; }
        public string ImgUlr { get; set; }
        public int HoldCount { get; set; }
        public bool IsCheckedOut { get; set; }

    }
}

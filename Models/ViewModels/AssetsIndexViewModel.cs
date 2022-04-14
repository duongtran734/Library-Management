namespace LibraryManagment.Models.ViewModels
{
    public class AssetsIndexViewModel
    {
        public int Id    { get; set; }
        public string ImgUrl { get; set; }
        public string Title { get; set; }
        public string AuthorOrDirector { get; set; }
        public string Type { get; set; }
        public string DeweyCallNumber { get; set; }
        public string NumberOfCopies { get; set; }
    }
}

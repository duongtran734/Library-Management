using System.ComponentModel.DataAnnotations;

namespace LibraryManagment.Models
{
    public class Video : LibraryAsset
    {
        [Required]
        public string Director { get; set; }
        //more ...
    }
}

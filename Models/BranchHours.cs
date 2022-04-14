
using System.ComponentModel.DataAnnotations;

namespace LibraryManagment.Models
{
    public class BranchHours
    {
        public int Id { get; set; }
        [Range(0,6)]
        public int DayOfWeek { get; set; }
        [Range(0,24)]
        public int OpenTime { get; set; }
        [Range(0,24)]
        public int CloseTime { get; set; }
    }
}

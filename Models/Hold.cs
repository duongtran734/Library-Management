﻿namespace LibraryManagment.Models
{
    public class Hold
    {
        public int Id { get; set; }
        public DateTime HoldPlaced { get; set; } //
        public virtual LibraryCard LibraryCard { get; set; }
        public virtual LibraryAsset LibraryAsset { get; set; }
    }
}

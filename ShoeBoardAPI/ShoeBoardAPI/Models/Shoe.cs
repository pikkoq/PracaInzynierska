﻿namespace ShoeBoardAPI.Models
{
    public class Shoe
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ShoeCatalogId { get; set; }
        public string Size { get; set; }
        public int ComfortRating { get; set; }
        public int StyleRating { get; set; }
        public string Season { get; set; }
        public string Review { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public ShoeCatalog ShoeCatalog { get; set; }


    }
}

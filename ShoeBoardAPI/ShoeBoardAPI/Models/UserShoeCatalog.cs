using System.ComponentModel.DataAnnotations;

namespace ShoeBoardAPI.Models
{
    public class UserShoeCatalog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Model_No { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Series { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        [MaxLength(2048)]
        public string Image_Path {  get; set; } = string.Empty;
        public string ShopUrl {  get; set; } = string.Empty;
        public DateTime Release_Date { get; set; }
        public string Main_Color { get; set; } = string.Empty;
        public string Colorway { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
    }
}

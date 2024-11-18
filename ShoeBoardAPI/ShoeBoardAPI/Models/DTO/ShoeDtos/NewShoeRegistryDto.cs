using System.ComponentModel.DataAnnotations;

namespace ShoeBoardAPI.Models.DTO.ShoeDtos
{
    public class NewShoeRegistryDto
    {
        public string Model_No { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Series { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        [MaxLength(2048)]
        public string Image_Path { get; set; } = string.Empty;
        public DateTime Release_Date { get; set; }
        public string Main_Color { get; set; } = string.Empty;
        public string Colorway { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
    }
}

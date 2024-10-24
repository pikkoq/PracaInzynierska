using System.ComponentModel.DataAnnotations;

namespace ShoeBoardAPI.Models.DTO.ShoeDtos
{
    public class EditShoeDetailsDto
    {
        public string Model_No { get; set; }
        public string Title { get; set; }
        public string Nickname { get; set; }
        public string Brand { get; set; }
        public string Series { get; set; }
        public string Gender { get; set; }
        [MaxLength(2048)]
        public string Image_Url { get; set; }
        public string Image_Path { get; set; }
        public DateTime Release_Date { get; set; } = DateTime.UtcNow;
        public string Main_Color { get; set; }
        public string Colorway { get; set; }
        public decimal Price { get; set; }
    }
}

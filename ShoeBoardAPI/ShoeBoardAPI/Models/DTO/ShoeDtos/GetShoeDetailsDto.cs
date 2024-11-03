using System.ComponentModel.DataAnnotations;

namespace ShoeBoardAPI.Models.DTO.ShoeDtos
{
    public class GetShoeDetailsDto
    {
        public int Id { get; set; }
        public string Model_No { get; set; }
        public string Title { get; set; }
        public string Nickname { get; set; }
        public string Brand { get; set; }
        public string Series { get; set; }
        public string Url_Link_Handler { get; set; }
        public string Gender { get; set; }
        [MaxLength(2048)]
        public string Image_Url { get; set; }
        public DateTime Release_Date { get; set; }
        public string Main_Color { get; set; }
        public string Colorway { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public int ComfortRating { get; set; }
        public int StyleRating { get; set; }
        public string Season { get; set; }
        public string Review { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

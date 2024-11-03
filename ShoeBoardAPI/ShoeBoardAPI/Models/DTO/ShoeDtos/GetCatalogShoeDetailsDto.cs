namespace ShoeBoardAPI.Models.DTO.ShoeDtos
{
    public class GetCatalogShoeDetailsDto
    {
        public string Model_No { get; set; }
        public string Title { get; set; }
        public string Nickname { get; set; }
        public string Brand { get; set; }
        public string Series { get; set; }
        public string ShopLink { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string MainColor { get; set; }
        public string Colorway { get; set; }
        public decimal Price { get; set; }
    }
}

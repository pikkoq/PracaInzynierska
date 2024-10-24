namespace ShoeBoardAPI.Models.DTO.ShoeDtos
{
    public class GetShoeSearchDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        public string Gender { get; set; }
        public string Image_Url { get; set; }
        public string Image_Path { get; set; }
        public string Price { get; set; }
    }
}

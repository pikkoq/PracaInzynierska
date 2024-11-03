namespace ShoeBoardAPI.Models.DTO.ShoeDtos
{
    public class GetAllUserShoesDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public string Image_Url { get; set; }
        public string Size { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

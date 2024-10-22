namespace ShoeBoardAPI.Models.DTO.ShoeDtos
{
    public class GetAllUserShoesDto
    {
        public string Id { get; set; }
        public string Size { get; set; }
        public int ComfortRating { get; set; }
        public int StyleRating { get; set; }
        public string Season { get; set; }
        public string Review { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

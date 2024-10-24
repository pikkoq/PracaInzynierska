namespace ShoeBoardAPI.Models.DTO.ShoeDtos
{
    public class EditUserShoeDto
    {
        public string Size { get; set; }
        public int ComfortRating { get; set; }
        public int StyleRating { get; set; }
        public string Season { get; set; }
        public string Review { get; set; }
    }
}

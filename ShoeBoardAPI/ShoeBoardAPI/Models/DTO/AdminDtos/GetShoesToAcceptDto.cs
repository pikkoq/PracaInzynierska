namespace ShoeBoardAPI.Models.DTO.AdminDtos
{
    public class GetShoesToAcceptDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Model_No { get; set; }
        public string Title { get; set; }
        public string Nickname { get; set; }
        public string Brand { get; set; }
        public string Series { get; set; }
        public string Gender { get; set; }
        public string Image_Path { get; set; }
        public DateTime Release_Date { get; set; }
        public string Main_Color { get; set; }
        public string Colorway { get; set; }
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

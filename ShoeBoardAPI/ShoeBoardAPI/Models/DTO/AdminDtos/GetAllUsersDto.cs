namespace ShoeBoardAPI.Models.DTO.AdminDtos
{
    public class GetAllUsersDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Bio {  get; set; }
        public string ProfilePicturePath { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

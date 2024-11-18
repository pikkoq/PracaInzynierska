using ShoeBoardAPI.Models.DTO.PostDtos;

namespace ShoeBoardAPI.Models.DTO.UserDtos
{
    public class GetUserProfileDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserProfileAvatar {  get; set; }
        public string Bio {  get; set; }
        public List<PostDto> Posts { get; set; }
    }
}

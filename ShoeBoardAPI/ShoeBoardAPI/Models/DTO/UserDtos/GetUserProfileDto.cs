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
        public bool IsFriend { get; set; }
        public bool IsRequestSent { get; set; }
        public bool IsRequestRecived { get; set; }
        public int? RequestId { get; set; }
    }
}

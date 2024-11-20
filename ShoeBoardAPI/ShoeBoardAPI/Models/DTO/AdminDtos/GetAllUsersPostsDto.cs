namespace ShoeBoardAPI.Models.DTO.AdminDtos
{
    public class GetAllUsersPostsDto
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string ProfilePicture {  get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }
        public string Size { get; set; }
        public int ComfortRating { get; set; }
        public int StyleRating { get; set; }
        public string Season { get; set; }
        public string Review { get; set; }
        public string Title { get; set; }
        public string ShoePhoto { get; set; }
        public int likeCount { get; set; }
        public int commentsCount { get; set; }
    }
}

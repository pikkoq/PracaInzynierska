namespace ShoeBoardAPI.Models.DTO.PostDtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }
        public int ShoeId { get; set; }
        public string UserId { get; set; }
        public List<CommentDto> Comments { get; set; }
        public int LikeCount { get; set; }
    }
}

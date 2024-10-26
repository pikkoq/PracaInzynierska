namespace ShoeBoardAPI.Models.DTO.PostDtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

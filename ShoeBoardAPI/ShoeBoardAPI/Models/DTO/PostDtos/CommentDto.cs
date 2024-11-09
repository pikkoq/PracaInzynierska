namespace ShoeBoardAPI.Models.DTO.PostDtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}

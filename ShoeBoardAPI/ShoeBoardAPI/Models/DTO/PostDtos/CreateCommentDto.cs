namespace ShoeBoardAPI.Models.DTO.PostDtos
{
    public class CreateCommentDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
    }
}

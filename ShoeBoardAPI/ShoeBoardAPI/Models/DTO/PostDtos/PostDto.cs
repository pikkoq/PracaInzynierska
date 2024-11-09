namespace ShoeBoardAPI.Models.DTO.PostDtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public int? ShoeCatalogId { get; set; }
        public string Username { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }
        public string Size { get; set; }
        public int ComfortRating { get; set; }
        public int StyleRating { get; set; }
        public string Season { get; set; }
        public string Review { get; set; }
        public string Title { get; set; }
        public string Image_Url { get; set; }
        public int LikeCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsLiked { get; set; }
    }
}

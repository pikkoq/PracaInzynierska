namespace ShoeBoardAPI.Models.DTO.FrindsDtos
{
    public class GetFriendRequestDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UserAvatar { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsAccepted { get; set; }
    }
}

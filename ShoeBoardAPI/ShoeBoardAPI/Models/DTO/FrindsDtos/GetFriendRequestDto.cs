namespace ShoeBoardAPI.Models.DTO.FrindsDtos
{
    public class GetFriendRequestDto
    {
        public string Username { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsAccepted { get; set; }
    }
}

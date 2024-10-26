namespace ShoeBoardAPI.Models.DTO.FrindsDtos
{
    public class GetFriendRequestDto
    {
        public int Id { get; set; }
        public string RequesterId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsAccepted { get; set; }
    }
}

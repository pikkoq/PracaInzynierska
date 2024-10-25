namespace ShoeBoardAPI.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public string RequesterId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsAccepted { get; set; }

        public User Requester { get; set; }
        public User Receiver { get; set; }
    }
}

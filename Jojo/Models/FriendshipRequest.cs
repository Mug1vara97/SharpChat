namespace Jojo.Models
{
    public class FriendshipRequest
    {
        public int Id { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public bool IsAccepted { get; set; }
    }

}

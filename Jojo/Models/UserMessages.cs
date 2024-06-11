namespace Jojo.Models
{
    public class ReadMessage
    {
        public int Id { get; set; }
        public int ChatMessageId { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
    }
}

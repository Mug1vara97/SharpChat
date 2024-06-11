namespace Jojo.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public DateTime Time { get; set; }
        public int ChatId { get; set; }
    }
}

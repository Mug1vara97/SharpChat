namespace Jojo.Models
{
    public class ChatPhoto
    {
        public int Id { get; set; }
        public string PhotoData { get; set; }
        public string Username { get; set; }
        public DateTime Time { get; set; }
        public int ChatId { get; set; }

        public Chat Chat { get; set; }
    }
}
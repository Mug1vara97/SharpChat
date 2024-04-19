namespace Jojo.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int NewsFeedItemId { get; set; }
        public string? Username { get; set; }
        public NewsFeedItem NewsFeedItem { get; set; }
    }
}

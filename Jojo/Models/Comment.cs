public class Comment
{
    public int Id { get; set; }
    public int NewsFeedItemId { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public DateTime PostedDate { get; set; }
    public NewsFeedItem NewsFeedItem { get; set; }
}
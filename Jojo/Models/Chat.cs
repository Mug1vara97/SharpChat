using Jojo.Models;

public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<ChatMessage> Messages { get; set; }
}
using Jojo.Models;

public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public List<string> AllowedUsers { get; set; }

    public List<ChatMessage> Messages { get; set; }

}
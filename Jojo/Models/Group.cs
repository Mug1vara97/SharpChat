using Jojo.Models;

public class GroupChat
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<User> Users { get; set; }
    public List<GroupChatMessage> Messages { get; set; }
}

public class GroupChatMessage
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string Username { get; set; }
    public DateTime Time { get; set; }
    public int GroupChatId { get; set; }
    public GroupChat GroupChat { get; set; }
}

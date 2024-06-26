﻿using Jojo.Models;

public class NewsFeedItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Content { get; set; }
    public byte[]? Photo { get; set; }
    public string? ContentType { get; set; }
    public DateTime PostedDate { get; set; }
    public string Author { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Like>? Likes { get; set; }

}
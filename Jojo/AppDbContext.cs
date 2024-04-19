using Microsoft.EntityFrameworkCore;

namespace Jojo.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<NewsFeedItem> NewsFeedItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friendship> Friends { get; set; }
        public DbSet<ChatPhoto> ChatPhotos { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<FriendshipRequest> FriendshipRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friendship>()
                .HasIndex(f => new { f.User1, f.User2 }).IsUnique();
        }
    }
}

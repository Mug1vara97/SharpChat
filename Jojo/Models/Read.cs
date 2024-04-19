using System.ComponentModel.DataAnnotations;

namespace Jojo.Models
{
    public class UnreadMessageCount
    {
        [Key]
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public int Count { get; set; }
    }
}

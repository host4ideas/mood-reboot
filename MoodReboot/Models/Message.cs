using System.ComponentModel.DataAnnotations;

namespace MoodReboot.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
    }
}
// Minuto 29:17
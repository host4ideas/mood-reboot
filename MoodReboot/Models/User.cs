using Microsoft.AspNetCore.Identity;

namespace MoodReboot.Models
{
    public class User : IdentityUser
    {
        // 1 To many relationship AppUser || Message
        public virtual ICollection<Message> Messages { get; set; }

        public User()
        {
            this.Messages = new HashSet<Message>();
        }
    }
}

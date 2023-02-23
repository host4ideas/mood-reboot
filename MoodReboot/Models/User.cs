using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodReboot.Models
{
    [Table("USER")]
    public class User : IdentityUser
    {
        [Key]
        [Column("USER_ID")]
        public int Id { get; set; }
        [Column("EMAIL")]
        public string Email { get; set; }
        [Column("PASSWORD")]
        public string Password { get; set; }
        [Column("USERNAME")]
        public string UserName { get; set; }
        [Column("FIRST_NAME")]
        public string FirstName { get; set; }
        [Column("LAST_NAME")]
        public string LastName { get; set; }
        [Column("SIGN_DATE")]
        public DateTime SignedDate { get; set; }
        [Column("IMAGE")]
        public string? Image { get; set; }
        [Column("ROLE")]
        public string Role { get; set; }
        [Column("LAST_SEEN")]
        public DateTime? LastSeen { get; set; }
        [Column("SALT")]
        public string Salt { get; set; }
    }
}

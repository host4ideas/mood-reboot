using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodReboot.Models
{
    [Table("USER")]
    public class User
    {
        [Key]
        [Column("USER_ID")]
        public int Id { get; set; }
        // Linq
        [Column("EMAIL")]
        public string Email { get; set; }
        // Linq
        [Column("PASSWORD")]
        public byte[] Password { get; set; }
        // Linq
        [Column("USERNAME")]
        public string UserName { get; set; }
        // Linq
        [Column("FIRST_NAME")]
        public string FirstName { get; set; }
        // Linq
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
        [Column("PASS_TEST")]
        public string PassTest { get; set; }
        [Column("APPROVED")]
        public bool Approved { get; set; }
    }
}

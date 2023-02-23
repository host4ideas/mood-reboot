using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodReboot.Models
{
    [Table("USER_GROUP")]
    public class UserChatGroup
    {
        [Key]
        [Column("USER_ID")]
        public int UserID { get; set; }
        [Column("GROUP_ID")]
        public int GroupId { get; set; }
        [Column("JOIN_DATE")]
        public DateTime JoinDate { get; set; }
    }
}

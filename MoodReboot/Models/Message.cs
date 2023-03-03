using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodReboot.Models
{
    [Table("MESSAGE")]
    public class Message
    {
        [Key]
        [Column("MESSAGE_ID")]
        public int Id { get; set; }
        [Column("GROUP_ID")]
        public int GroupId { get; set; }
        [Column("TEXT")]
        public string? Text { get; set; }
        [Column("USER_ID")]
        public int UserID { get; set; }
        [Column("DATE_POSTED")]
        public DateTime DatePosted { get; set; }
        [Column("FILE_ID")]
        public int? FileId { get; set; }
        [Column("SEEN")]
        public Boolean Seen { get; set; }
        [Column("USERNAME")]
        public string UserName { get; set; }
        // Extra
        public AppFile? Attachment { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodReboot.Models
{
    [Table("FILE")]
    public class File
    {
        [Key]
        [Column("FILE_ID")]
        public int Id { get; set; }
        [Column("USER_ID")]
        public int? UserId { get; set; }
        [Column("MIME_TYPE")]
        public string MimeType { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodReboot.Models
{
    [Table("CONTENT")]
    public class Content
    {
        [Key]
        [Column("CONTENT_ID")]
        public int Id { get; set; }
        [Column("TEXT")]
        public string? Text { get; set; }
        [Column("GROUP_CONTENT_ID")]
        public int ContentGroupId { get; set; }
        [Column("FILE_ID")]
        public int? FileId { get; set; }
        // Extra
        public AppFile? File { get; set; }
    }
}

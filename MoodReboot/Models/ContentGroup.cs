using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodReboot.Models
{
    [Table("CONTENT_COURSE")]
    public class ContentGroup
    {
        [Key]
        [Column("GROUP_CONTENT_ID")]
        public int ContentGroupId { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        [Column("IS_VISIBLE")]
        public Boolean IsVisible { get; set; }
        [Column("COURSE_ID")]
        public int CourseID { get; set; }
    }
}

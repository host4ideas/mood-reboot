using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodReboot.Models
{
    [Table("COURSE")]
    public class Course
    {
        [Key]
        [Column("COURSE_ID")]
        public int Id { get; set; }
        [Column("DATE_PUBLISHED")]
        public DateTime DatePublished { get; set; }
        [Column("DATE_MODIFIED")]
        public DateTime? DateModified { get; set; }
        [Column("GROUP_ID")]
        public int? GroupId { get; set; }
        [Column("DESCRIPTION")]
        public string? Description { get; set; }
        [Column("IMAGE")]
        public string? Image { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("CENTER_ID")]
        public int CenterId { get; set; }
        [Column("IS_VISIBLE")]
        public Boolean IsVisible { get; set; }
        // Extra
        public List<ContentGroup>? Contents { get; set; }
        public User? Author { get; set; }
    }
}

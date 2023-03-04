using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodReboot.Models
{
    [Table("USER_COURSE")]
    public class UserCourse
    {
        [Key]
        [Column("USER_ID")]
        public int UserId { get; set; }
        [Column("COURSE_ID")]
        public int CourseId { get; set; }
        [Column("IS_EDITOR")]
        public Boolean IsEditor { get; set; }
    }
}

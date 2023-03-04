namespace MoodReboot.Models
{
    public class CourseDetailsModel
    {
        public Course Course { get; set; }
        public List<ContentGroup> ContentGroups { get; set; }
        public Boolean IsEditor { get; set; }
    }
}

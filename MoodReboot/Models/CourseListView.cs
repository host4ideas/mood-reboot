namespace MoodReboot.Models
{
    public class CourseListView
    {
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime? DateModified { get; set; }
        public string Name { get; set; }
        public Boolean IsEditor { get; set; }
        public string CenterName { get; set; }
        public int Id { get; set; }
    }
}

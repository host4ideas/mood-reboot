using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryCourses
    {
        public List<Course> GetAllCourses();
        public Task<Course?> FindCourse(int id);
        public List<CourseListView> GetUserCourses(int id);
        public List<CourseListView> GetCenterCourses(int id);
        public Task CreateCourse(string name, string? description, string? image, int? isVisible);
        public Task DeleteCourse(int id);
        public Task UpdateCourse(int id, string description, string image, string name, Boolean isVisible);
    }
}

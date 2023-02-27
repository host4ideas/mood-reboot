using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryCourses
    {
        public List<Course> GetAllCourses();
        public Task<Course?> FindCourse(int id);
        public List<Course> GetUserCourses(int id);
        public Task CreateCourse(string name, string? description, string? image, int? isVisible);
        public Task DeleteCourse(int id);
        public Task UpdateCourse(Course course);
    }
}

using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryCourses
    {
        public List<Course> GetAllCourses();
        public Task<Course?> FindCourse(int id);
        public List<CourseListView> GetUserCourses(int id);
        public List<CourseListView> GetCenterCourses(int id);
        public List<CourseUsersModel> GetCourseUsers(int courseId);
        public List<CourseListView> CenterCoursesListView(int centerId);
        public Task RemoveCourseUserAsync(int courseId, int userId);
        public Task RemoveCourseEditorAsync(int courseId, int userId);
        public Task AddCourseEditorAsync(int courseId, int userId);
        public Task CreateCourse(string name, string? description, string? image, int? isVisible);
        public Task DeleteCourse(int id);
        public Task UpdateCourse(int id, string description, string image, string name, Boolean isVisible);
    }
}

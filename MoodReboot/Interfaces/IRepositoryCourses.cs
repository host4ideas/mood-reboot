using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryCourses
    {
        public List<Course> GetAllCourses();
        public Task<Course?> FindCourse(int id);
        public Task<UserCourse?> FindUserCourse(int userId, int courseId);
        public List<CourseListView> GetUserCourses(int id);
        public List<CourseListView> GetCenterCourses(int id);
        public List<CourseUsersModel> GetCourseUsers(int courseId);
        public List<CourseListView> CenterCoursesListView(int centerId);
        public Task RemoveCourseUserAsync(int courseId, int userId);
        public Task RemoveCourseEditorAsync(int courseId, int userId);
        public Task AddCourseEditorAsync(int courseId, int userId);
        public Task<bool> AddCourseUserAsync(int courseId, int userId, bool isEditor, string? password);
        public Task CreateCourse(int centerId, string name, bool isVisible, string? description = null, string? image = null, string? password = null);
        public Task DeleteCourse(int id);
        public Task UpdateCourse(int id, string description, string image, string name, Boolean isVisible);
    }
}

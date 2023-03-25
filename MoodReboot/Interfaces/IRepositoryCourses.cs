using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryCourses
    {
        public Task<int> GetMaxCourse();
        public Task<List<Course>> GetAllCourses();
        public Task<Course?> FindCourse(int id);
        public Task<UserCourse?> FindUserCourse(int userId, int courseId);
        public Task<List<CourseListView>> GetUserCourses(int id);
        public Task<List<CourseListView>> GetCenterCourses(int id);
        public Task<List<CourseUsersModel>> GetCourseUsers(int courseId);
        public Task<List<CourseListView>> GetEditorCenterCourses(int userId, int centerId);
        public Task<List<CourseListView>> CenterCoursesListView(int centerId);
        public Task RemoveCourseUserAsync(int courseId, int userId);
        public Task RemoveCourseEditorAsync(int courseId, int userId);
        public Task AddCourseEditorAsync(int courseId, int userId);
        public Task<bool> AddCourseUserAsync(int courseId, int userId, bool isEditor);
        public Task<bool> AddCourseUserAsync(int courseId, int userId, bool isEditor, string password);
        public Task DeleteCourse(int id);
        public Task UpdateCourseVisibility(int courseId);
        public Task UpdateCourse(int id, string description, string image, string name, Boolean isVisible);
    }
}

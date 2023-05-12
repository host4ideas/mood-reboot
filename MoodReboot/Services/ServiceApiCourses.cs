using NugetMoodReboot.Interfaces;
using NugetMoodReboot.Models;

namespace MoodReboot.Services
{
    public class ServiceApiCourses : IRepositoryCourses
    {
        public Task AddCourseEditorAsync(int courseId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddCourseUserAsync(int courseId, int userId, bool isEditor)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddCourseUserAsync(int courseId, int userId, bool isEditor, string password)
        {
            throw new NotImplementedException();
        }

        public Task<List<CourseListView>> CenterCoursesListViewAsync(int centerId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCourseAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Course?> FindCourseAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserCourse?> FindUserCourseAsync(int userId, int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Course>> GetAllCoursesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<CourseListView>> GetCenterCoursesAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CourseUsersModel>> GetCourseUsersAsync(int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CourseListView>> GetEditorCenterCoursesAsync(int userId, int centerId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMaxCourseAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<CourseListView>> GetUserCoursesAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveCourseEditorAsync(int courseId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveCourseUserAsync(int courseId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCourseAsync(int id, string description, string image, string name, bool isVisible)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCourseVisibilityAsync(int courseId)
        {
            throw new NotImplementedException();
        }
    }
}

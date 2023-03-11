using Microsoft.AspNetCore.Mvc;
using MoodReboot.Extensions;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IRepositoryCourses repositoryCourses;
        private readonly IRepositoryContent repositoryContent;
        private readonly IRepositoryContentGroups repositoryContentGroups;

        public CoursesController(IRepositoryCourses repositoryCourses, IRepositoryContent repositoryContent, IRepositoryContentGroups repositoryContentGroups)
        {
            this.repositoryCourses = repositoryCourses;
            this.repositoryContent = repositoryContent;
            this.repositoryContentGroups = repositoryContentGroups;
        }

        public IActionResult UserCourses(int userId)
        {
            List<CourseListView> courses = this.repositoryCourses.GetUserCourses(userId);
            return View(courses);
        }

        public async Task<IActionResult> DeleteCourseUser(int courseId, int userId)
        {
            await this.repositoryCourses.RemoveCourseUserAsync(courseId, userId);
            return RedirectToAction("CourseDetails", new { id = courseId });
        }

        public async Task<IActionResult> DeleteCourseEditor(int courseId, int userId)
        {
            await this.repositoryCourses.RemoveCourseEditorAsync(courseId, userId);
            return RedirectToAction("CourseDetails", new { id = courseId });
        }

        public async Task<IActionResult> AddCourseEditor(int courseId, int userId)
        {
            await this.repositoryCourses.AddCourseEditorAsync(courseId, userId);
            return RedirectToAction("CourseDetails", new { id = courseId });
        }

        public async Task<IActionResult> UpdateCourse(int userId, int courseId, string description, string image, string name, bool isVisible)
        {
            await this.repositoryCourses.UpdateCourse(courseId, description, image, name, isVisible);
            return RedirectToAction("UserCourses", new { id = userId });
        }

        public IActionResult CenterCourses(int centerId)
        {
            List<CourseListView> courses = this.repositoryCourses.GetCenterCourses(centerId);
            return RedirectToAction("Index", new { Courses = courses });
        }

        public IActionResult GetAllCourses()
        {
            List<Course> courses = this.repositoryCourses.GetAllCourses();
            return RedirectToAction("Index", new { Courses = courses });
        }

        public async Task<IActionResult> CourseDetails(int id)
        {
            List<ContentGroup> contentGroups = this.repositoryContentGroups.GetCourseContentGroups(id);

            foreach (ContentGroup group in contentGroups)
            {
                List<Content> content = this.repositoryContent.GetContentByGroup(group.ContentGroupId);
                group.Contents = content;
            }

            Course? course = await this.repositoryCourses.FindCourse(id);

            if (course == null || contentGroups == null)
            {
                SessionUser currentLoggedUser = HttpContext.Session.GetObject<SessionUser>("user")!;
                return RedirectToAction("Courses", new { userId = currentLoggedUser.Id });
            }

            List<CourseUsersModel> courseUsers = this.repositoryCourses.GetCourseUsers(course.Id);

            CourseDetailsModel details = new()
            {
                ContentGroups = contentGroups,
                Course = course,
                CourseUsers = courseUsers,
                IsEditor = true
            };

            return View(details);
        }

        public IActionResult DeleteCourse(int id)
        {
            this.repositoryCourses.DeleteCourse(id);
            return RedirectToAction("Index");
        }
    }
}

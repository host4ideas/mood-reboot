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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserCourses()
        {
            UserSession? userSession = HttpContext.Session.GetObject<UserSession>("USER");
            List<CourseListView> courses = this.repositoryCourses.GetUserCourses(userSession.UserId);
            return View("Index", courses);
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
            return View("Index", courses);
        }

        public IActionResult GetAllCourses()
        {
            List<Course> courses = this.repositoryCourses.GetAllCourses();
            return View("Index", courses);
        }

        public async Task<IActionResult> CourseEnrollment(int courseId)
        {
            Course? course = await this.repositoryCourses.FindCourse(courseId);

            // The course doesn't exist
            if (course == null)
            {
                SessionUser user = HttpContext.Session.GetObject<SessionUser>("USER")!;
                return RedirectToAction("Courses", new { userId = user.Id });
            }

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CourseEnrollment(int courseId, int userId, string password, bool isEditor = false)
        {
            Course? course = await this.repositoryCourses.FindCourse(courseId);

            if (course != null)
            {
                // If the course doesn't have password, enroll the user in the course
                if (course.Password == null)
                {
                    SessionUser currentLoggedUser = HttpContext.Session.GetObject<SessionUser>("USER")!;
                    await this.repositoryCourses.AddCourseUserAsync(courseId, currentLoggedUser.Id, isEditor, null);
                    return RedirectToAction("CourseDetails", new { courseId });
                }
                // If the course has password
                bool added = await this.repositoryCourses.AddCourseUserAsync(courseId, userId, isEditor, password);
                if (added == true)
                {
                    ViewData["ERROR"] = "Contraseña del curso incorrecta";
                    return RedirectToAction("CourseEnrollment", new { courseId });
                }
            }
            // Fallback to user's courses
            return RedirectToAction("UserCourses", new { id = courseId });
        }

        public async Task<IActionResult> CourseDetails(int courseId)
        {
            UserSession? userSession = HttpContext.Session.GetObject<UserSession>("USER");

            if (userSession != null)
            {
                UserCourse? userCourse = await this.repositoryCourses.FindUserCourse(userSession.UserId, courseId);

                if (userCourse != null)
                {
                    List<ContentGroup> contentGroups = this.repositoryContentGroups.GetCourseContentGroups(courseId);

                    foreach (ContentGroup group in contentGroups)
                    {
                        List<Content> content = this.repositoryContent.GetContentByGroup(group.ContentGroupId);
                        group.Contents = content;
                    }

                    Course? course = await this.repositoryCourses.FindCourse(courseId);

                    if (course == null || contentGroups == null)
                    {
                        SessionUser currentLoggedUser = HttpContext.Session.GetObject<SessionUser>("user")!;
                        return RedirectToAction("UserCourses", new { userId = currentLoggedUser.Id });
                    }

                    List<CourseUsersModel> courseUsers = this.repositoryCourses.GetCourseUsers(course.Id);

                    CourseDetailsModel details;

                    if (userCourse.IsEditor)
                    {
                        details = new()
                        {
                            ContentGroups = contentGroups,
                            Course = course,
                            CourseUsers = courseUsers,
                            IsEditor = true
                        };
                    }
                    else
                    {
                        details = new()
                        {
                            ContentGroups = contentGroups,
                            Course = course,
                            IsEditor = false
                        };
                    }

                    return View(details);
                }
            }
            return RedirectToAction("CourseEnrollment", new { courseId });
        }

        public IActionResult DeleteCourse(int id)
        {
            this.repositoryCourses.DeleteCourse(id);
            return RedirectToAction("UserCourses");
        }
    }
}

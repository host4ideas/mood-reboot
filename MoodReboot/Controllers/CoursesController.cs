using Microsoft.AspNetCore.Mvc;
using MoodReboot.Extensions;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using MvcCoreSeguridadEmpleados.Filters;
using System.Collections.Generic;
using System.Security.Claims;

namespace MoodReboot.Controllers
{
    [AuthorizeUsers]
    public class CoursesController : Controller
    {
        private readonly IRepositoryCourses repositoryCourses;
        private readonly IRepositoryContent repositoryContent;
        private readonly IRepositoryContentGroups repositoryContentGroups;
        private readonly IRepositoryUsers repositoryUsers;

        public CoursesController(IRepositoryCourses repositoryCourses, IRepositoryContent repositoryContent, IRepositoryContentGroups repositoryContentGroups, IRepositoryUsers repositoryUsers)
        {
            this.repositoryCourses = repositoryCourses;
            this.repositoryContent = repositoryContent;
            this.repositoryContentGroups = repositoryContentGroups;
            this.repositoryUsers = repositoryUsers;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserCourses()
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<CourseListView> courses = await this.repositoryCourses.GetUserCourses(userId);
            return View("Index", courses);
        }

        public async Task<IActionResult> DeleteCourseUser(int courseId, int userId)
        {
            Course? course = await this.repositoryCourses.FindCourse(courseId);
            if (course != null)
            {
                await this.repositoryCourses.RemoveCourseUserAsync(courseId, userId);
                if (course.GroupId.HasValue)
                {
                    await this.repositoryUsers.RemoveChatUser(userId, course.GroupId.Value);
                }
            }
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

        public async Task<IActionResult> CenterCourses(int centerId)
        {
            List<CourseListView> courses = await this.repositoryCourses.GetCenterCourses(centerId);
            return View("Index", courses);
        }

        public async Task<IActionResult> GetAllCourses()
        {
            List<Course> courses = await this.repositoryCourses.GetAllCourses();
            return View("Index", courses);
        }

        public async Task<IActionResult> CourseEnrollment(int courseId)
        {
            Course? course = await this.repositoryCourses.FindCourse(courseId);

            // The course doesn't exist
            if (course == null)
            {
                return RedirectToAction("UserCourses");
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
                    await this.repositoryCourses.AddCourseUserAsync(courseId, userId, isEditor);
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

        [AuthorizeUsers]
        public async Task<IActionResult> CourseDetails(int courseId)
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            UserCourse? userCourse = await this.repositoryCourses.FindUserCourse(userId, courseId);

            if (userCourse != null)
            {
                List<ContentGroup> contentGroups = this.repositoryContentGroups.GetCourseContentGroups(courseId);

                foreach (ContentGroup group in contentGroups)
                {
                    List<Content> contentList = await this.repositoryContent.GetContentByGroup(group.ContentGroupId);
                    List<ContentListModel> contentFileList = new();

                    foreach (Content ctn in contentList)
                    {
                        ContentListModel contentFile = new()
                        {
                            ContentGroupId = ctn.ContentGroupId,
                            Id = ctn.Id,
                            Text = ctn.Text,
                            FileId = ctn.FileId
                        };

                        if (ctn.FileId != null)
                        {
                            contentFile.File = await this.repositoryUsers.FindFile(ctn.FileId.Value);
                        }

                        contentFileList.Add(contentFile);
                    }

                    group.Contents = contentFileList;
                }

                Course? course = await this.repositoryCourses.FindCourse(courseId);

                if (course == null || contentGroups == null)
                {
                    return RedirectToAction("UserCourses", new { userId });
                }

                // Add to last seen courses
                List<LastSeenCourse>? lastSeenCourses = HttpContext.Session.GetObject<List<LastSeenCourse>>("LAST_COURSES");

                if (lastSeenCourses == null)
                {
                    lastSeenCourses = new();
                }
                else if (lastSeenCourses.Count == 5)
                {
                    lastSeenCourses.RemoveAt(0);
                }

                lastSeenCourses.Add(new LastSeenCourse()
                {
                    Id = course.Id,
                    Description = course.Description,
                    Image = course.Image,
                    Name = course.Name,
                });

                // Save without duplicates
                HttpContext.Session.SetObject("LAST_COURSES", lastSeenCourses.DistinctBy(x => x.Id).ToList());

                // Course users
                List<CourseUsersModel> courseUsers = await this.repositoryCourses.GetCourseUsers(course.Id);

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
            // If the user is not already enrolled to the course
            return RedirectToAction("CourseEnrollment", new { courseId });
        }

        public IActionResult DeleteCourse(int id)
        {
            this.repositoryCourses.DeleteCourse(id);
            return RedirectToAction("UserCourses");
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using MoodReboot.Extensions;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using MvcCoreSeguridadEmpleados.Filters;
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
                    List<Content> content = await this.repositoryContent.GetContentByGroup(group.ContentGroupId);

                    foreach (Content ctn in content)
                    {
                        if (ctn.FileId != null)
                        {
                            ctn.File = await this.repositoryUsers.FindFile(ctn.FileId.Value);
                        }
                    }

                    group.Contents = content;
                }

                Course? course = await this.repositoryCourses.FindCourse(courseId);

                if (course == null || contentGroups == null)
                {
                    return RedirectToAction("UserCourses", new { userId });
                }

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
            return RedirectToAction("CourseEnrollment", new { courseId });
        }

        public IActionResult DeleteCourse(int id)
        {
            this.repositoryCourses.DeleteCourse(id);
            return RedirectToAction("UserCourses");
        }
    }
}

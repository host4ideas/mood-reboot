using Microsoft.AspNetCore.Mvc;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using MvcCoreSeguridadEmpleados.Filters;

namespace MoodReboot.Controllers
{
    [AuthorizeUsers(Policy = "ADMIN_ONLY")]
    public class AdminController : Controller
    {
        private readonly IRepositoryCenters repositoryCenters;
        private readonly IRepositoryCourses repositoryCourses;
        private readonly IRepositoryUsers repositoryUsers;

        public AdminController(IRepositoryCenters repositoryCenters, IRepositoryCourses repositoryCourses, IRepositoryUsers repositoryUsers)
        {
            this.repositoryCenters = repositoryCenters;
            this.repositoryCourses = repositoryCourses;
            this.repositoryUsers = repositoryUsers;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Requests()
        {
            return View();
        }

        public async Task<IActionResult> CenterRequests()
        {
            List<Center> centers = await this.repositoryCenters.GetPendingCenters();
            return PartialView("_PendingCentersPartial", centers);
        }

        public async Task<IActionResult> UserRequests()
        {
            List<User> users = await this.repositoryUsers.GetPendingUsers();
            return PartialView("_PendingUsersPartial", users);
        }

        public async Task<IActionResult> ApproveCenter(int centerId)
        {
            await this.repositoryCenters.ApproveCenter(centerId);
            return RedirectToAction("Requests");
        }

        public async Task<IActionResult> ApproveUser(int userId)
        {
            await this.repositoryUsers.ApproveUser(userId);
            return RedirectToAction("Requests");
        }

        public IActionResult Users()
        {
            List<User> users = this.repositoryUsers.GetAllUsers();
            return View(users);
        }

        public async Task<IActionResult> DeleteUser(int userId)
        {
            await this.repositoryUsers.DeleteUser(userId);
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> Centers()
        {
            List<CenterListView> centers = await this.repositoryCenters.GetAllCenters();
            return View(centers);
        }

        public async Task<IActionResult> DeleteCenter(int centerId)
        {
            await this.repositoryCenters.DeleteCenter(centerId);
            return RedirectToAction("Centers");
        }

        public IActionResult Courses()
        {
            List<Course> courses = this.repositoryCourses.GetAllCourses();
            return View(courses);
        }

        public async Task<IActionResult> UpdateCourse(int courseId, string description, string image, string name, bool isVisible)
        {
            await this.repositoryCourses.UpdateCourse(courseId, description, image, name, isVisible);
            return RedirectToAction("Courses");
        }

        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            await this.repositoryCourses.DeleteCourse(courseId);
            return RedirectToAction("Courses");
        }
    }
}

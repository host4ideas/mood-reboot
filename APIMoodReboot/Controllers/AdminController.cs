using Microsoft.AspNetCore.Mvc;
using APIMoodReboot.Interfaces;
using NugetMoodReboot.Helpers;
using NugetMoodReboot.Models;

namespace APIMoodReboot.Controllers
{
    //[AuthorizeUsers(Policy = "ADMIN_ONLY")]
    public class AdminController : Controller
    {
        private readonly IRepositoryCenters repositoryCenters;
        private readonly IRepositoryCourses repositoryCourses;
        private readonly IRepositoryUsers repositoryUsers;
        private readonly HelperMail helperMail;

        public AdminController(IRepositoryCenters repositoryCenters, IRepositoryCourses repositoryCourses, IRepositoryUsers repositoryUsers, HelperMail helperMail)
        {
            this.repositoryCenters = repositoryCenters;
            this.repositoryCourses = repositoryCourses;
            this.repositoryUsers = repositoryUsers;
            this.helperMail = helperMail;
        }

        public async Task<IActionResult> CenterRequests()
        {
            List<Center> centers = await this.repositoryCenters.GetPendingCentersAsync();
            return PartialView("_PendingCentersPartial", centers);
        }

        public async Task<IActionResult> UserRequests()
        {
            List<AppUser> users = await this.repositoryUsers.GetPendingUsersAsync();
            return PartialView("_PendingUsersPartial", users);
        }

        public async Task<IActionResult> ApproveCenter(int centerId)
        {
            Center? center = await this.repositoryCenters.FindCenterAsync(centerId);
            if (center != null)
            {
                await this.repositoryCenters.ApproveCenterAsync(center);

                string protocol = HttpContext.Request.IsHttps ? "https" : "http";
                string domainName = HttpContext.Request.Host.Value.ToString();
                string baseUrl = protocol + domainName;
                await this.helperMail.SendMailAsync(center.Email, "Centro aprobado", "Tu centro ha sido aprobado en la plataforma APIMoodReboot, puedes iniciar sesión en tu perfil y empezar a administrarlo", baseUrl);
            }
            return RedirectToAction("Requests");
        }

        public async Task<IActionResult> ApproveUser(int userId)
        {
            AppUser? user = await this.repositoryUsers.FindUserAsync(userId);
            if (user != null)
            {
                await this.repositoryUsers.ApproveUserAsync(user);
                string protocol = HttpContext.Request.IsHttps ? "https" : "http";
                string domainName = HttpContext.Request.Host.Value.ToString();
                string baseUrl = protocol + domainName;
                await this.helperMail.SendMailAsync(user.Email, "Usuario aprobado", "Tu cuenta en APIMoodReboot ha sido activada, por favor, inicia sesión con tu cuenta para empezar a utilizar nuestra plataforma.", baseUrl);
            }
            return RedirectToAction("Requests");
        }

        public async Task<IActionResult> Users()
        {
            List<AppUser> users = await this.repositoryUsers.GetAllUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> DeleteUser(int userId)
        {
            await this.repositoryUsers.DeleteUserAsync(userId);
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> Centers()
        {
            List<CenterListView> centers = await this.repositoryCenters.GetAllCentersAsync();
            return View(centers);
        }

        public async Task<IActionResult> DeleteCenter(int centerId)
        {
            await this.repositoryCenters.DeleteCenterAsync(centerId);
            return RedirectToAction("Centers");
        }

        public async Task<IActionResult> Courses()
        {
            List<Course> courses = await this.repositoryCourses.GetAllCoursesAsync();
            return View(courses);
        }

        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            await this.repositoryCourses.DeleteCourseAsync(courseId);
            return RedirectToAction("Courses");
        }

        public async Task<IActionResult> CourseVisibility(int courseId)
        {
            await this.repositoryCourses.UpdateCourseVisibilityAsync(courseId);
            return RedirectToAction("Courses");
        }
    }
}

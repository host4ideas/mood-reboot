using Microsoft.AspNetCore.Mvc;
using MoodReboot.Helpers;
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
        private readonly HelperMail helperMail;

        public AdminController(IRepositoryCenters repositoryCenters, IRepositoryCourses repositoryCourses, IRepositoryUsers repositoryUsers, HelperMail helperMail)
        {
            this.repositoryCenters = repositoryCenters;
            this.repositoryCourses = repositoryCourses;
            this.repositoryUsers = repositoryUsers;
            this.helperMail = helperMail;
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
            List<AppUser> users = await this.repositoryUsers.GetPendingUsers();
            return PartialView("_PendingUsersPartial", users);
        }

        public async Task<IActionResult> ApproveCenter(int centerId)
        {
            Center? center = await this.repositoryCenters.FindCenter(centerId);
            if (center != null)
            {
                await this.repositoryCenters.ApproveCenter(center);
                await this.helperMail.SendMailAsync(center.Email, "Centro aprobado", "Tu centro ha sido aprobado en la plataforma MoodReboot, puedes iniciar sesión en tu perfil y empezar a administrarlo");
            }
            return RedirectToAction("Requests");
        }

        public async Task<IActionResult> ApproveUser(int userId)
        {
            AppUser? user = await this.repositoryUsers.FindUser(userId);
            if (user != null)
            {
                await this.repositoryUsers.ApproveUser(user);
                await this.helperMail.SendMailAsync(user.Email, "Usuario aprobado", "Tu cuenta en MoodReboot ha sido activada, por favor, inicia sesión con tu cuenta para empezar a utilizar nuestra plataforma.");
            }
            return RedirectToAction("Requests");
        }

        public IActionResult Users()
        {
            List<AppUser> users = this.repositoryUsers.GetAllUsers();
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

        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            await this.repositoryCourses.DeleteCourse(courseId);
            return RedirectToAction("Courses");
        }
    }
}

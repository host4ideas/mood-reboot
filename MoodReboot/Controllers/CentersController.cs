using Microsoft.AspNetCore.Mvc;
using MoodReboot.Extensions;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using System.Security.Claims;

namespace MoodReboot.Controllers
{
    public class CentersController : Controller
    {
        private readonly IRepositoryCenters repositoryCenters;
        private readonly IRepositoryCourses repositoryCourses;

        public CentersController(IRepositoryCenters repositoryCenters, IRepositoryCourses repositoryCourses)
        {
            this.repositoryCenters = repositoryCenters;
            this.repositoryCourses = repositoryCourses;
        }

        public async Task<IActionResult> Index()
        {
            List<CenterListView> centers = await this.repositoryCenters.GetAllCenters();
            return View(centers);
        }

        public async Task<IActionResult> CenterDetails(int id)
        {
            Center? center = await this.repositoryCenters.FindCenter(id);

            if (center == null)
            {
                return View();
            }

            List<CourseListView> courses = this.repositoryCourses.CenterCoursesListView(id);

            ViewData["CENTER"] = center;
            return View(courses);
        }

        public async Task<IActionResult> UserCenters()
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<CenterListView> centers = await this.repositoryCenters.GetUserCentersAsync(userId);
            return View("Index", centers);
        }
    }
}

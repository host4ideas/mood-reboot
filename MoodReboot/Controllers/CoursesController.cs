using Microsoft.AspNetCore.Mvc;

namespace MoodReboot.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CourseDetails()
        {
            return View();
        }
    }
}

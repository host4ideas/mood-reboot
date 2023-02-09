using Microsoft.AspNetCore.Mvc;

namespace MoodReboot.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult Centers()
        {
            return View();
        }

        public IActionResult Courses()
        {
            return View();
        }

        // Also allow search comments from a user
        public IActionResult Comments()
        {
            return View();
        }
    }
}

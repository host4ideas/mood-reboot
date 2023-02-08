using Microsoft.AspNetCore.Mvc;

namespace MoodReboot.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Courses()
        {
            return View();
        }
    }
}

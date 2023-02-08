using Microsoft.AspNetCore.Mvc;

namespace MoodReboot.Controller
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace MoodReboot.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}

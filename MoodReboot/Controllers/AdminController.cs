using Microsoft.AspNetCore.Mvc;

namespace MoodReboot.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

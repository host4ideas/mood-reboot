using Microsoft.AspNetCore.Mvc;

namespace MoodReboot.Controllers
{
    public class CentersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CenterDetails()
        {
            return View();
        }
    }
}

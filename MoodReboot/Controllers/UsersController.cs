using Microsoft.AspNetCore.Mvc;

namespace MoodReboot.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}

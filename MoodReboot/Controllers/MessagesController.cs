using Microsoft.AspNetCore.Mvc;

namespace MoodReboot.Controllers
{
    public class MessagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

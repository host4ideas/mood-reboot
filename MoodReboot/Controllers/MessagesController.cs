using Microsoft.AspNetCore.Mvc;

namespace MoodReboot.Controllers
{
    public class MessagesController : Controller
    {
        public IActionResult Chats()
        {
            return View();
        }

        public IActionResult CourseForum()
        {
            return View();
        }
    }
}

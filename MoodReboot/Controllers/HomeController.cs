using Markdig;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

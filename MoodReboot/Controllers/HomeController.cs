using Markdig;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MoodReboot.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {

            var result = Markdown.ToHtml("This is a text with some *emphasis*");

            Debug.WriteLine(result);

            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace MoodReboot.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(string email, string username, string firstName, string lastName, string password, string repeatPassword)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Signout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}

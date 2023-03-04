using Microsoft.AspNetCore.Mvc;
using MoodReboot.Extensions;
using MoodReboot.Models;

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
        public Boolean Login(string password, string username = "", string email = "")
        {
            if (password == "admin" && (username == "admin" || email == "admin@admin.com"))
            {
                HttpContext.Session.SetObject("user", new SessionUser() { Email = "admin@admin.com", Id = 0, UserName = "admin", Role = "ADMIN" });
                return true;
            }
            else if (password == "user" && (username == "user" || email == "user@user.com"))
            {
                HttpContext.Session.SetObject("user", new SessionUser() { Email = "user@user.com", Id = 1, UserName = "user", Role = "USER" });
                return true;
            }
            return false;
        }

        [HttpPost]
        public IActionResult Signout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}

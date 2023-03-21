using Microsoft.AspNetCore.Mvc;
using MoodReboot.Extensions;
using MoodReboot.Helpers;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Controllers
{
    public class AuthController : Controller
    {
        private readonly HelperFile helperFile;
        private readonly IRepositoryUsers repositoryUsers;

        public AuthController(HelperFile helperFile, IRepositoryUsers repositoryUsers)
        {
            this.helperFile = helperFile;
            this.repositoryUsers = repositoryUsers;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            User? user = await this.repositoryUsers.LoginUser(email, password);
            if (user == null)
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
            return View(user);
        }
    }
}
 
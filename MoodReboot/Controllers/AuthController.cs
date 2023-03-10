using Microsoft.AspNetCore.Mvc;
using MoodReboot.Extensions;
using MoodReboot.Helpers;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using MvcCoreUtilidades.Helpers;

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

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp
            (string nombre, string firstName, string lastName, string email, string password, IFormFile imagen)
        {
            string maximo = this.repositoryUsers.GetMaximo().ToString();

            string fileName = "image_" + maximo;

            string path = await this.helperFile.UploadFileAsync(imagen, Folders.Images, fileName);

            await this.repositoryUsers.RegisterUser(nombre, firstName, lastName, email, password, path);
            ViewData["MENSAJE"] = "Usuario registrado correctamente";
            return View();
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

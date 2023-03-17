using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MoodReboot.Helpers;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using MvcCoreUtilidades.Helpers;
using System.Security.Claims;

namespace MoodReboot.Controllers
{
    public class ManagedController : Controller
    {
        private readonly HelperFile helperFile;
        private readonly HelperPath helperPath;
        private readonly IRepositoryUsers repositoryUsers;

        public ManagedController(HelperFile helperFile, HelperPath helperPath, IRepositoryUsers repositoryUsers)
        {
            this.helperFile = helperFile;
            this.repositoryUsers = repositoryUsers;
            this.helperPath = helperPath;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usernameOrEmail, string password)
        {
            // Pass to findUser the userId
            User? user = await this.repositoryUsers.LoginUser(usernameOrEmail, password);

            if (user == null)
            {
                ViewData["MESSAGE"] = "Usuario/password incorrectos";
                return View();
            }

            ClaimsIdentity identity = new(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            Claim claimName = new(ClaimTypes.Name, user.UserName);
            identity.AddClaim(claimName);

            Claim claimEmail = new(ClaimTypes.Email, user.Email);
            identity.AddClaim(claimEmail);

            Claim claimId = new(ClaimTypes.NameIdentifier, user.Id.ToString());
            identity.AddClaim(claimId);

            string? userImage = user.Image;
            if (userImage == null)
            {
                userImage = this.helperPath.MapPath("default_user_logo.svg", Folders.Logos);
            }
            Claim claimImage = new("IMAGE", userImage);
            identity.AddClaim(claimImage);

            ClaimsPrincipal userPrincipal = new(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp
            (string nombre, string firstName, string lastName, string email, string password, IFormFile imagen)
        {
            int maximo = await this.repositoryUsers.GetMaxUser();

            string fileName = "image_" + maximo;

            string path = await this.helperFile.UploadFileAsync(imagen, Folders.Images, fileName);

            await this.repositoryUsers.RegisterUser(nombre, firstName, lastName, email, password, path);
            ViewData["MENSAJE"] = "Usuario registrado correctamente";
            return View();
        }
    }
}

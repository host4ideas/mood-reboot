using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MoodReboot.Helpers;
using MoodReboot.Interfaces;
using MoodReboot.Models;
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

        public IActionResult AccessError()
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

            Claim claimRole = new(ClaimTypes.Role, user.Role);
            identity.AddClaim(claimRole);

            Claim claimImage = new("IMAGE", user.Image);
            identity.AddClaim(claimImage);

            ClaimsPrincipal userPrincipal = new(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

            string controller = TempData["controller"].ToString();
            string action = TempData["action"].ToString();

            return RedirectToAction(action, controller);
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
            (string nombre, string firstName, string lastName, string email, string password, IFormFile? imagen)
        {
            string path = "default_user_logo.svg";

            if (imagen != null)
            {
                int maximo = await this.repositoryUsers.GetMaxUser();

                string fileName = "image_" + maximo;

                path = await this.helperFile.UploadFileAsync(imagen, Folders.Images, fileName);
            }

            await this.repositoryUsers.RegisterUser(nombre, firstName, lastName, email, password, path);
            ViewData["MENSAJE"] = "Usuario registrado correctamente";
            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            if (await this.repositoryUsers.IsEmailAvailable(email) == false)
            {
                return Json($"Email {email} ya está en uso.");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyUsername(string userName)
        {
            if (await this.repositoryUsers.IsUsernameAvailable(userName) == false)
            {
                return Json($"Nick {userName} ya está en uso.");
            }

            return Json(true);
        }
    }
}

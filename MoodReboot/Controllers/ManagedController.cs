using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MoodReboot.Helpers;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Security.Claims;

namespace MoodReboot.Controllers
{
    public class ManagedController : Controller
    {
        private readonly HelperFile helperFile;
        private readonly HelperPath helperPath;
        private readonly HelperMail helperMail;
        private readonly IRepositoryUsers repositoryUsers;

        public ManagedController(HelperFile helperFile, HelperPath helperPath, HelperMail helperMail, IRepositoryUsers repositoryUsers)
        {
            this.helperFile = helperFile;
            this.repositoryUsers = repositoryUsers;
            this.helperPath = helperPath;
            this.helperMail = helperMail;
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
            // BBDD
            string path = "default_user_logo.svg";

            if (imagen != null)
            {
                int maximo = await this.repositoryUsers.GetMaxUser();

                string fileName = "image_" + maximo;

                path = await this.helperFile.UploadFileAsync(imagen, Folders.Images, fileName);
            }

            int userId = await this.repositoryUsers.RegisterUser(nombre, firstName, lastName, email, password, path);
            string token = await this.repositoryUsers.CreateUserAction(userId);

            // Confirmation mail
            string url = Url.Action("ApproveUserEmail", "Users", new { userId, token })!;

            List<MailLink> links = new()
            {
                new MailLink()
                {
                    LinkText = "Confirmar cuenta",
                    Link = url
                }
            };
            await this.helperMail.SendMailAsync(email, "Confirmación de cuenta", "Se ha solicitado una petición para crear una cuenta en MoodReboot con este correo electrónico. Pulsa el siguiente enlace para confirmarla. Si no has sido tu el solicitante no te procupes, la petición será cancelada en un período de 24hrs.", links);

            ViewData["MENSAJE"] = "Revisa tu correo electrónico";
            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> EmailExists(string email)
        {
            if (await this.repositoryUsers.IsEmailAvailable(email) == false)
            {
                return Json(true);
            }
            return Json($"Email {email} no pertenece a ningún usuario de la plataforma.");
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

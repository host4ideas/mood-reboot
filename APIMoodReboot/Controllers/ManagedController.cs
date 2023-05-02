using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using APIMoodReboot.Interfaces;
using System.Security.Claims;
using NugetMoodReboot.Models;
using NugetMoodReboot.Helpers;

namespace APIMoodReboot.Controllers
{
    public class ManagedController : Controller
    {
        private readonly HelperFile helperFile;
        private readonly HelperMail helperMail;
        private readonly IRepositoryUsers repositoryUsers;

        public ManagedController(HelperFile helperFile, HelperMail helperMail, IRepositoryUsers repositoryUsers)
        {
            this.helperFile = helperFile;
            this.repositoryUsers = repositoryUsers;
            this.helperMail = helperMail;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usernameOrEmail, string password)
        {
            // Pass to findUser the userId
            AppUser? user = await this.repositoryUsers.LoginUserAsync(usernameOrEmail, password);

            if (user == null)
            {
                ViewData["MESSAGE"] = "Usuario/password incorrectos";
                return View();
            }
            else if (user.Approved == false)
            {
                string token = await this.repositoryUsers.CreateUserActionAsync(user.Id);
                string resendUrl = Url.Action("ResendConfirmationEmail", "Managed", new { userId = user.Id, token });
                ViewData["MESSAGE"] = $"Este usuario no ha sido validado, <a class='text-blue-700 hover:underline dark:text-blue-500' href='{resendUrl}'>Quiero recibir de nuevo la confirmación por correo</a>";
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

        public async Task<IActionResult> ResendConfirmationEmail(int userId, string token)
        {
            AppUser? user = await this.repositoryUsers.FindUserAsync(userId);

            if (user != null)
            {
                // Confirmation mail
                string protocol = HttpContext.Request.IsHttps ? "https" : "http";
                string domainName = HttpContext.Request.Host.Value.ToString();
                string baseUrl = protocol + domainName;
                string url = Url.Action("ApproveUserEmail", "Users", new { userId, token }, protocol)!;

                List<MailLink> links = new()
            {
                new MailLink()
                {
                    LinkText = "Confirmar cuenta",
                    Link = url
                }
            };
                await this.helperMail.SendMailAsync(user.Email, "Confirmación de cuenta", "Se ha solicitado una petición para crear una cuenta en MoodReboot con este correo electrónico. Pulsa el siguiente enlace para confirmarla. Si no has sido tu el solicitante no te procupes, la petición será cancelada en un período de 24hrs.", links, baseUrl);

                ViewData["SUCCESS"] = "Correo de confirmación enviado";
                return View("Login");
            }
            ViewData["MESSAGE"] = "Datos inválidos";
            return View("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp
            (string userName, string firstName, string lastName, string email, string password, IFormFile image)
        {
            string? path = "default_user_logo.svg";

            // Upload profile image
            if (image != null)
            {
                int maximo = await this.repositoryUsers.GetMaxUserAsync();

                string fileName = "image_" + maximo;

                path = await this.helperFile.UploadFileAsync(image, Folders.ProfileImages, FileTypes.Image, fileName);

                if (path == null)
                {
                    ViewData["ERROR"] = "Error al subir archivo";
                    return View();
                }
            }

            // BBDD
            int userId = await this.repositoryUsers.RegisterUserAsync(userName, firstName, lastName, email, password, path);

            // Confirmation token
            string token = await this.repositoryUsers.CreateUserActionAsync(userId);

            // Confirmation mail
            string protocol = HttpContext.Request.IsHttps ? "https" : "http";
            string domainName = HttpContext.Request.Host.Value.ToString();
            string baseUrl = protocol + domainName;
            string url = Url.Action("ApproveUserEmail", "Users", new { userId, token }, protocol)!;

            List<MailLink> links = new()
            {
                new MailLink()
                {
                    LinkText = "Confirmar cuenta",
                    Link = url
                }
            };
            await this.helperMail.SendMailAsync(email, "Confirmación de cuenta", "Se ha solicitado una petición para crear una cuenta en APIMoodReboot con este correo electrónico. Pulsa el siguiente enlace para confirmarla. Si no has sido tu el solicitante no te procupes, la petición será cancelada en un período de 24hrs.", links, baseUrl);

            ViewData["SUCCESS"] = "Revisa tu correo electrónico";
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MoodReboot.Helpers;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using MvcCoreSeguridadEmpleados.Filters;
using System.Security.Claims;

namespace MoodReboot.Controllers
{
    public class UsersController : Controller
    {
        private readonly IRepositoryUsers repositoryUsers;
        private readonly HelperFile helperFile;
        private readonly HelperMail helperMail;

        public UsersController(IRepositoryUsers repositoryUsers, HelperFile helperFile, HelperMail helperMail)
        {
            this.repositoryUsers = repositoryUsers;
            this.helperFile = helperFile;
            this.helperMail = helperMail;
        }

        public async Task<List<Tuple<string, int>>> SearchUsers(string pattern)
        {
            return await this.repositoryUsers.SearchUsers(pattern);
        }

        [AuthorizeUsers]
        public async Task<IActionResult> Profile()
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            AppUser? user = await this.repositoryUsers.FindUser(userId);
            return View(user);
        }

        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> Profile(int userId, string userName, string firstName, string lastName, IFormFile image)
        {
            if (image != null)
            {
                string fileName = "image_" + userId;
                await this.helperFile.UploadFileAsync(image, Folders.ProfileImages, FileTypes.Image, fileName);
                await this.repositoryUsers.UpdateUserBasics(userId, userName, firstName, lastName, fileName);
                return RedirectToAction("Profile", new { userId });
            }

            await this.repositoryUsers.UpdateUserBasics(userId, userName, firstName, lastName);
            return RedirectToAction("Profile", new { userId });
        }

        /// <summary>
        /// Approve user register request
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IActionResult> ApproveUserEmail(int userId, string token)
        {
            UserAction? userAction = await this.repositoryUsers.FindUserAction(userId, token);

            if (userAction != null)
            {
                DateTime limitDate = userAction.RequestDate.AddHours(24);
                // Expired request - passed 24hrs
                if (DateTime.Now > limitDate)
                {
                    await this.repositoryUsers.RemoveUserAction(userAction);
                }
                else
                {
                    await this.repositoryUsers.RemoveUserAction(userAction);
                    await this.repositoryUsers.ApproveUser(userId);
                }
            }
            return RedirectToAction("Logout", "Managed");
        }

        #region CHANGE EMAIL

        public async Task<IActionResult> ChangeEmail(int userId, string token)
        {
            AppUser? user = await this.repositoryUsers.FindUser(userId);

            if (user != null)
            {
                ViewData["ERROR"] = "Petición invalida";
            }
            else
            {
                ViewData["TOKEN"] = token;
                ViewData["USERID"] = userId;
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail(int userId, string token, string email)
        {
            UserAction? userAction = await this.repositoryUsers.FindUserAction(userId, token);

            if (userAction != null)
            {
                DateTime limitDate = userAction.RequestDate.AddHours(24);
                // Expired request - passed 24hrs
                if (DateTime.Now > limitDate)
                {
                    await this.repositoryUsers.RemoveUserAction(userAction);
                    ViewData["ERROR"] = "Petición ha expirado";
                    return View();
                }
                else
                {
                    await this.repositoryUsers.RemoveUserAction(userAction);
                    await this.repositoryUsers.UpdateUserEmail(userId, email);
                }
            }
            return RedirectToAction("Logout", "Managed");
        }

        [AuthorizeUsers]
        public async Task RequestChangeEmail(int userId, string email)
        {
            string token = await this.repositoryUsers.CreateUserAction(userId);
            string url = Url.Action("ChangeEmail", "Users", new { userId, token })!;

            List<MailLink> links = new()
            {
                new MailLink()
                {
                    LinkText = "Confirmar cambio email",
                    Link = url
                }
            };
            string protocol = HttpContext.Request.IsHttps ? "https" : "http";
            string domainName = HttpContext.Request.Host.Value.ToString();
            string baseUrl = protocol + domainName;
            await this.helperMail.SendMailAsync(email, "Cambio de datos", "Se ha solicitado una petición para cambiar el correo electrónico de la cuenta asociada. Pulsa el siguiente enlace para confirmarla. Una vez cambiada deberás de iniciar sesión con el nuevo correo electónico, si surge cualquier problema o tienes alguna duda, contáctanos a: moodreboot@gmail.com. <br/><br/> Si no eres solicitante no te procupes, la petición será cancelada en un período de 24hrs.", links, baseUrl);
        }

        #endregion

        #region CHANGE PASSWORD

        public async Task<IActionResult> ChangePassword(int userId, string token)
        {
            AppUser? user = await this.repositoryUsers.FindUser(userId);

            if (user != null)
            {
                ViewData["ERROR"] = "Petición invalida";
            }
            else
            {
                ViewData["TOKEN"] = token;
                ViewData["USERID"] = userId;
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(int userId, string token, string password)
        {
            UserAction? userAction = await this.repositoryUsers.FindUserAction(userId, token);

            if (userAction != null)
            {
                DateTime limitDate = userAction.RequestDate.AddHours(24);
                // Expired request - passed 24hrs
                if (DateTime.Now > limitDate)
                {
                    await this.repositoryUsers.RemoveUserAction(userAction);
                    ViewData["ERROR"] = "Petición ha expirado";
                    return View();
                }
                else
                {
                    await this.repositoryUsers.RemoveUserAction(userAction);
                    await this.repositoryUsers.UpdateUserPassword(userId, password);
                }
            }
            return RedirectToAction("Logout", "Managed");
        }

        [AuthorizeUsers]
        [HttpPost]
        public async Task RequestChangePassword(int userId, string email)
        {
            string token = await this.repositoryUsers.CreateUserAction(userId);
            string url = Url.Action("ChangePassword", "Users", new { userId, token })!;

            List<MailLink> links = new()
            {
                new MailLink()
                {
                    LinkText = "Confirmar cambio de contraseña",
                    Link = url
                }
            };
            string protocol = HttpContext.Request.IsHttps ? "https" : "http";
            string domainName = HttpContext.Request.Host.Value.ToString();
            string baseUrl = protocol + domainName;
            await this.helperMail.SendMailAsync(email, "Cambio de datos", "Se ha solicitado una petición para cambiar la contraseña de la cuenta asociada. Pulsa el siguiente enlace para confirmarla. Una vez cambiada deberás de iniciar sesión con el nuevo correo electónico, si surge cualquier problema o tienes alguna duda, contáctanos a: moodreboot@gmail.com. <br/><br/> Si no eres solicitante no te procupes, la petición será cancelada en un período de 24hrs.", links, baseUrl);
        }

        #endregion
    }
}

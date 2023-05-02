using Microsoft.AspNetCore.Mvc;
using APIMoodReboot.Interfaces;
using System.Security.Claims;
using NugetMoodReboot.Helpers;
using NugetMoodReboot.Models;

namespace APIMoodReboot.Controllers
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

        [HttpGet("[action]/{pattern}")]
        public async Task<ActionResult<List<Tuple<string, int>>>> SearchUsers(string pattern)
        {
            return await this.repositoryUsers.SearchUsersAsync(pattern);
        }

        //[AuthorizeUsers]
        [HttpGet("[action]")]
        public async Task<ActionResult<AppUser>> Profile()
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            AppUser? user = await this.repositoryUsers.FindUserAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //[AuthorizeUsers]
        [HttpPut("[action]")]
        public async Task<ActionResult> Profile(UpdateProfileModel updateProfile)
        {
            if (updateProfile.Image != null && updateProfile.Image.Length > 0)
            {
                string fileName = "image_" + updateProfile.UserId;
                await this.helperFile.UploadFileAsync(updateProfile.Image, Folders.ProfileImages, FileTypes.Image, fileName);
                await this.repositoryUsers.UpdateUserBasicsAsync(updateProfile.UserId, updateProfile.Username, updateProfile.FirstName, updateProfile.LastName, fileName);
                return NoContent();
            }

            await this.repositoryUsers.UpdateUserBasicsAsync(updateProfile.UserId, updateProfile.Username, updateProfile.FirstName, updateProfile.LastName);
            return NoContent();
        }

        /// <summary>
        /// Approve user register request
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IActionResult> ApproveUserEmail(int userId, string token)
        {
            UserAction? userAction = await this.repositoryUsers.FindUserActionAsync(userId, token);

            if (userAction != null)
            {
                DateTime limitDate = userAction.RequestDate.AddHours(24);
                // Expired request - passed 24hrs
                if (DateTime.Now > limitDate)
                {
                    await this.repositoryUsers.RemoveUserActionAsync(userAction);
                }
                else
                {
                    await this.repositoryUsers.RemoveUserActionAsync(userAction);
                    await this.repositoryUsers.ApproveUserAsync(userId);
                }
            }
            return RedirectToAction("Logout", "Managed");
        }

        #region CHANGE EMAIL

        public async Task<IActionResult> ChangeEmail(int userId, string token)
        {
            AppUser? user = await this.repositoryUsers.FindUserAsync(userId);

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
            UserAction? userAction = await this.repositoryUsers.FindUserActionAsync(userId, token);

            if (userAction != null)
            {
                DateTime limitDate = userAction.RequestDate.AddHours(24);
                // Expired request - passed 24hrs
                if (DateTime.Now > limitDate)
                {
                    await this.repositoryUsers.RemoveUserActionAsync(userAction);
                    ViewData["ERROR"] = "Petición ha expirado";
                    return View();
                }
                else
                {
                    await this.repositoryUsers.RemoveUserActionAsync(userAction);
                    await this.repositoryUsers.UpdateUserEmailAsync(userId, email);
                }
            }
            return RedirectToAction("Logout", "Managed");
        }

        //[AuthorizeUsers]
        public async Task RequestChangeEmail(int userId, string email)
        {
            string token = await this.repositoryUsers.CreateUserActionAsync(userId);
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
            AppUser? user = await this.repositoryUsers.FindUserAsync(userId);

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
            UserAction? userAction = await this.repositoryUsers.FindUserActionAsync(userId, token);

            if (userAction != null)
            {
                DateTime limitDate = userAction.RequestDate.AddHours(24);
                // Expired request - passed 24hrs
                if (DateTime.Now > limitDate)
                {
                    await this.repositoryUsers.RemoveUserActionAsync(userAction);
                    ViewData["ERROR"] = "Petición ha expirado";
                    return View();
                }
                else
                {
                    await this.repositoryUsers.RemoveUserActionAsync(userAction);
                    await this.repositoryUsers.UpdateUserPasswordAsync(userId, password);
                }
            }
            return RedirectToAction("Logout", "Managed");
        }

        //[AuthorizeUsers]
        [HttpPost]
        public async Task RequestChangePassword(int userId, string email)
        {
            string token = await this.repositoryUsers.CreateUserActionAsync(userId);
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

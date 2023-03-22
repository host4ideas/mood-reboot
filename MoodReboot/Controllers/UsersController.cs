using Microsoft.AspNetCore.Mvc;
using MoodReboot.Extensions;
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

        [AuthorizeUsers]
        public async Task<IActionResult> Profile()
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            User? user = await this.repositoryUsers.FindUser(userId);
            return View(user);
        }

        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> Profile(int userId, string userName, string firstName, string lastName, IFormFile image)
        {
            //if (image != null)
            //{
            //    string fileName = "image_" + userId;
            //    await this.helperFile.UploadFileAsync(image, Folders.Images, fileName);
            //    await this.repositoryUsers.UpdateUserBasics(userId, userName, firstName, lastName, fileName);
            //    return RedirectToAction("Profile", new { userId });
            //}

            //await this.repositoryUsers.UpdateUserBasics(userId, userName, firstName, lastName);
            return RedirectToAction("Profile", new { userId });
        }

        public async Task<List<Tuple<string, int>>> SearchUsers(string pattern)
        {
            return await this.repositoryUsers.SearchUsers(pattern);
        }

        public async Task ApproveUserEmail(int userId, string token)
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
        }

        public async Task ApproveUserEmail(int userId, string token, string newEmail)
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
                    await this.repositoryUsers.UpdateUserEmail(userId, newEmail);
                }
            }
        }

        [AuthorizeUsers]
        public async Task RequestChangeEmail(int userId, string email, string token)
        {
            string url = Url.Action("ApproveUserEmail", "Users", new { userId, token })!;

            List<MailLink> links = new()
            {
                new MailLink()
                {
                    LinkText = "Confirmar cambio email",
                    Link = url
                }
            };
            await this.repositoryUsers.CreateUserAction(userId);
            await this.helperMail.SendMailAsync(email, "Cambio de datos", "Se ha solicitado una petición para cambiar el correo electrónico de la cuenta asociada. Pulsa el siguiente enlace para confirmarla. Una vez cambiada deberás de iniciar sesión con el nuevo correo electónico, si surge cualquier problema o tienes alguna duda, contáctanos a: moodreboot@gmail.com. <br/><br/> Si no eres solicitante no te procupes, la petición será cancelada en un período de 24hrs.", links);
        }
    }
}

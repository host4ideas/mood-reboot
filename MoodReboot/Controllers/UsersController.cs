using Microsoft.AspNetCore.Mvc;
using MoodReboot.Extensions;
using MoodReboot.Helpers;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using MvcCoreUtilidades.Helpers;

namespace MoodReboot.Controllers
{
    public class UsersController : Controller
    {
        private readonly IRepositoryUsers repositoryUsers;
        private readonly HelperFile helperFile;

        public UsersController(IRepositoryUsers repositoryUsers, HelperFile helperFile)
        {
            this.repositoryUsers = repositoryUsers;
            this.helperFile = helperFile;
        }

        public async Task<IActionResult> Profile(int userId)
        {
            User? user = await this.repositoryUsers.FindUser(userId);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(int userId, string userName, string firstName, string lastName, IFormFile image)
        {
            if (image != null)
            {
                string fileName = "image_" + userId;
                await this.helperFile.UploadFileAsync(image, Folders.Images, fileName);
                await this.repositoryUsers.UpdateUserBasics(userId, userName, firstName, lastName, fileName);
                return RedirectToAction("Profile", new { userId });
            }

            await this.repositoryUsers.UpdateUserBasics(userId, userName, firstName, lastName);
            return RedirectToAction("Profile", new { userId });
        }

        public async Task<List<Tuple<string, int>>> SearchUsers(string pattern)
        {
            return await this.repositoryUsers.SearchUsers(pattern);
        }
    }
}

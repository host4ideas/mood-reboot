using Microsoft.AspNetCore.Mvc;
using MoodReboot.Extensions;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Controllers
{
    public class MessagesController : Controller
    {
        private IRepositoryUsers repositoryUsers;

        public MessagesController(IRepositoryUsers repositoryUsers)
        {
            this.repositoryUsers = repositoryUsers;
        }

        public IActionResult Chats()
        {
            return View();
        }

        public IActionResult CourseForum()
        {
            return View();
        }

        public IActionResult ChatWindowPartial(int userId)
        {
            List<ChatGroup> groups = this.repositoryUsers.GetUserChatGroups(userId);
            return PartialView("_ChatWindowPartial", groups);
        }
    }
}

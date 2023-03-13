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

        public IActionResult ChatErrorPartial(string errorMessage)
        {
            ViewData["ERROR"] = errorMessage;
            return View();
        }

        public IActionResult ChatWindowPartial(int userId)
        {
            List<ChatGroup> groups = this.repositoryUsers.GetUserChatGroups(userId);
            return PartialView("_ChatWindowPartial", groups);
        }

        public List<Message> GetChatMessages(int chatGroupId)
        {
            List<Message> messages = this.repositoryUsers.GetMessagesByGroup(chatGroupId);
            return messages;
        }
    }
}

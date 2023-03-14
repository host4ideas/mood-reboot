using Microsoft.AspNetCore.Mvc;
using MoodReboot.Extensions;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IRepositoryUsers repositoryUsers;

        public MessagesController(IRepositoryUsers repositoryUsers)
        {
            this.repositoryUsers = repositoryUsers;
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

        public IActionResult ChatNotificationsPartial(int userId)
        {
            List<Message> unseen = this.repositoryUsers.GetUnseenMessages(userId);
            return PartialView("_ChatNotificationsPartial", unseen);
        }

        public List<Message> GetChatMessages(int chatGroupId)
        {
            List<Message> messages = this.repositoryUsers.GetMessagesByGroup(chatGroupId);
            return messages;
        }

        public List<Message> GetUnseenMessages(int userId)
        {
            return this.repositoryUsers.GetUnseenMessages(userId);
        }

        public async Task UpdateChatLastSeen(int chatGroupId)
        {
            UserSession? userSession = HttpContext.Session.GetObject<UserSession>("USER");
            if (userSession != null)
            {
                await this.repositoryUsers.UpdateChatLastSeen(chatGroupId, userSession.UserId);
            }
        }
    }
}

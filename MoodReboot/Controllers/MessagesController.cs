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

            UserSession? user = HttpContext.Session.GetObject<UserSession>("USER");
            if (user != null)
            {
                List<Message> messages = this.repositoryUsers.GetUnseenMessages(user.UserId);

                ViewBag.UnseenCount = messages.Count;
                ViewBag.UnseenMessages = messages;
            }

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

        public List<Message> GetMessages(int userId)
        {
            return this.repositoryUsers.GetUnseenMessages(userId);
        }

        public void UpdateChatLastSeen(int chatGroupId)
        {
            
        }
    }
}

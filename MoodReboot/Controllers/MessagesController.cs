using Microsoft.AspNetCore.Mvc;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using MvcCoreSeguridadEmpleados.Filters;
using System.Security.Claims;

namespace MoodReboot.Controllers
{
    [AuthorizeUsers]
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
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            await this.repositoryUsers.UpdateChatLastSeen(chatGroupId, userId);
        }

        public async Task CreateChatGroup(List<int> userIds, string? groupName = "NEW CHAT GROUP")
        {
            // Add current user to the list of users
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            userIds.Add(userId);

            // List without duplicates
            HashSet<int> userIdsNoDups = new(userIds);

            if (userIds.Count == 2)
            {
                await this.repositoryUsers.NewChatGroup(userIdsNoDups);
            }
            else if (userIds.Count > 2)
            {
                await this.repositoryUsers.NewChatGroup(userIdsNoDups, userId, groupName);
            }
        }

        [HttpPost]
        public Task AddUsersToChat(int chatGroupId, List<int> userIds)
        {
            return this.repositoryUsers.AddUsersToChat(chatGroupId, userIds);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateChatGroup(ChatGroup chatGroup)
        {
            await this.repositoryUsers.UpdateChatGroup(chatGroup.Id, chatGroup.Name);
            return RedirectToAction("Index", "Home");
        }

        public async Task DeleteChatGroup(int chatGroupId)
        {
            await this.repositoryUsers.RemoveChatGroup(chatGroupId);
        }

        public Task RemoveUserFromChat(int userId, int chatGroupId)
        {
            return this.repositoryUsers.RemoveChatUser(userId, chatGroupId);
        }
    }
}

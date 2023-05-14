using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NugetMoodReboot.Models;
using Microsoft.AspNetCore.Authorization;
using NugetMoodReboot.Interfaces;

namespace APIMoodReboot.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IRepositoryUsers repositoryUsers;

        public MessagesController(IRepositoryUsers repositoryUsers)
        {
            this.repositoryUsers = repositoryUsers;
        }

        [HttpGet("{chatGroupId}")]
        public async Task<ActionResult<List<Message>>> GetChatMessages(int chatGroupId)
        {
            return await this.repositoryUsers.GetMessagesByGroupAsync(chatGroupId);
        }

        [HttpGet]
        public async Task<ActionResult<List<ChatGroup>>> GetUserChatGroups()
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return await this.repositoryUsers.GetUserChatGroupsAsync(userId);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<Message>>> GetUnseenMessages(int userId)
        {
            return await this.repositoryUsers.GetUnseenMessagesAsync(userId);
        }

        [HttpPut("{chatGroupId}")]
        public async Task<ActionResult> UpdateChatLastSeen(int chatGroupId)
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            await this.repositoryUsers.UpdateChatLastSeenAsync(chatGroupId, userId);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage(CreateChatMessageApiModel model)
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            await this.repositoryUsers.CreateMessageAsync(userId, model.GroupChatId, model.UserName, model.Text, model.FileId);
            return CreatedAtAction(null, null);
        }

        [HttpPost]
        public async Task<ActionResult> CreateChatGroup(CreateChatGroupModel createChatGroup)
        {
            // Add current user to the list of users and set it as admin
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            createChatGroup.UserIds.Add(userId);

            // List without duplicates
            HashSet<int> userIdsNoDups = new(createChatGroup.UserIds);

            if (createChatGroup.UserIds.Count == 2)
            {
                await this.repositoryUsers.NewChatGroupAsync(userIdsNoDups);
            }
            else if (createChatGroup.UserIds.Count > 2)
            {
                await this.repositoryUsers.NewChatGroupAsync(userIdsNoDups, userId, createChatGroup.GroupName);
            }

            return CreatedAtAction(null, null);
        }

        [HttpPost]
        public async Task<ActionResult> AddUsersToChat(AddUsersChatApiModel model)
        {
            await this.repositoryUsers.AddUsersToChatAsync(model.ChatGroupId, model.UserIds);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateChatGroup(ChatGroup chatGroup)
        {
            await this.repositoryUsers.UpdateChatGroupAsync(chatGroup.Id, chatGroup.Name);
            return NoContent();
        }

        [HttpDelete("{chatGroupId}")]
        public async Task<ActionResult> DeleteChatGroup(int chatGroupId)
        {
            await this.repositoryUsers.RemoveChatGroupAsync(chatGroupId);
            return NoContent();
        }

        [HttpDelete("{userId}/{chatGroupId}")]
        public async Task<ActionResult> RemoveUserFromChat(int userId, int chatGroupId)
        {
            await this.repositoryUsers.RemoveChatUserAsync(userId, chatGroupId);
            return NoContent();
        }
    }
}

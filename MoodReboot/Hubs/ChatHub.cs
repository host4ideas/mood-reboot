using AngleSharp.Common;
using Microsoft.AspNetCore.SignalR;
using MoodReboot.Extensions;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using static System.Net.Mime.MediaTypeNames;

namespace MoodReboot.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IRepositoryUsers repositoryUsers;

        public ChatHub(IRepositoryUsers repositoryUsers)
        {
            this.repositoryUsers = repositoryUsers;
        }

        public async Task SendMessage(int userId, int groupChatId, string userName, string text, string fileId, string seen)
        {
            await Clients.All.SendAsync("ReceiveMessage", userName, text);
        }

        public async Task SendMessageToGroup(string userId, string groupChatId, string userName, string text, string seen)
        {
            // Send message to group
            await Clients.Group(groupChatId.ToString()).SendAsync(
                "ReceiveMessageGroup",
                userName,
                groupChatId,
                DateTime.Now,
                text);

            //bool boolSeen = false;

            //if (seen == "1")
            //{
            //    boolSeen = true;
            //}

            //// Store the mesage in the DDBB
            //await this.repositoryUsers.CreateMessage(
            //    userId: int.Parse(userId),
            //    groupChatId: int.Parse(groupChatId),
            //    userName: userName,
            //    text: text,
            //    seen: boolSeen);
        }

        public async Task AddToGroup(string groupName, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("GroupNotification", $"{userName} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName, string userName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("GroupNotification", $"{userName} has left the group {groupName}.");
        }

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            if (httpContext != null)
            {
                // Get the userId param
                int userId = int.Parse(httpContext.Request.Query["userId"]);

                UserSession? userSession = httpContext.Session.GetObject<UserSession>("USER");

                // Check if the user is logged
                if (userSession != null && userSession.UserId == userId)
                {
                    // If the user is logged in add it to its chat groups
                    List<UserChatGroup> groups = this.repositoryUsers.GetUserChatGroups(userId);

                    foreach (UserChatGroup group in groups)
                    {
                        this.AddToGroup(group.GroupId.ToString(), userSession.UserName).Wait();
                    }
                }
            }

            return base.OnConnectedAsync();
        }
    }
}

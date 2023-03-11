using AngleSharp.Common;
using Microsoft.AspNetCore.SignalR;
using MoodReboot.Extensions;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using System.Text.RegularExpressions;

namespace MoodReboot.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IRepositoryUsers repositoryUsers;

        public ChatHub(IRepositoryUsers repositoryUsers)
        {
            this.repositoryUsers = repositoryUsers;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToGroup(string groupId, string user, string date, string message)
        {
            await Clients.Group(groupId).SendAsync("ReceiveMessageGroup", user, groupId, date, message);
        }

        // Cuando un cliente haga click en los chats, añadirlo a los grupos de los cursos donde esta matriculado
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("GroupNotification", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("GroupNotification", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            if (httpContext != null)
            {
                // Get the userId param
                int userId = int.Parse(httpContext.Request.Query["userId"]);

                // Check if the user is logged
                UserSession? userSession = httpContext.Session.GetObject<UserSession>("USER");

                if (userSession != null && userSession.UserId == userId)
                {
                    // If the user is logged in add it to its chat groups
                    List<int> groupsIds = this.repositoryUsers.GetUserChatGroups(userId);

                    foreach (int groupId in groupsIds)
                    {
                        this.AddToGroup(groupId.ToString()).Wait();
                    }
                }
            }

            // Testing
            //this.AddToGroup("1").Wait();

            return base.OnConnectedAsync();
        }
    }
}

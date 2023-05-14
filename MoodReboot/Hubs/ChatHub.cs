﻿using Microsoft.AspNetCore.SignalR;
using MoodReboot.Services;
using NugetMoodReboot.Models;
using System.Security.Claims;

namespace MoodReboot.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ServiceApiUsers serviceUsers;

        public ChatHub(ServiceApiUsers serviceUsers)
        {
            this.serviceUsers = serviceUsers;
        }

        public async Task SendMessage(int userId, int groupChatId, string userName, string text, string fileId)
        {
            await Clients.All.SendAsync("ReceiveMessage", userName, text);
        }

        public async Task SendMessageToGroup(string userId, string groupChatId, string userName, string text)
        {
            // Send message to group
            await Clients.Group(groupChatId.ToString()).SendAsync(
                "ReceiveMessageGroup",
                userName,
                groupChatId,
                DateTime.Now,
                text);

            // Store the mesage in the DDBB
            await this.serviceUsers.CreateMessageAsync(
                groupChatId: int.Parse(groupChatId),
                userName: userName,
                text: text);
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
            // Check if the user is logged
            if (Context.User.Identity.IsAuthenticated == true)
            {
                int userId = int.Parse(Context.User.FindFirstValue(ClaimTypes.NameIdentifier));
                string userName = Context.User.FindFirstValue(ClaimTypes.Name);

                // If the user is logged in add it to its chat groups
                List<ChatGroup> groups = this.serviceUsers.GetUserChatGroupsAsync(userId).Result;

                foreach (ChatGroup group in groups)
                {
                    this.AddToGroup(group.Id.ToString(), userName).Wait();
                }
            }

            return base.OnConnectedAsync();
        }
    }
}

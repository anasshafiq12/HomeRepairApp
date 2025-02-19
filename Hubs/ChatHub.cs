﻿using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace HouseRepairApp.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> Users = new ConcurrentDictionary<string, string>();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatHub(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Store user's connection and email
        public async Task SetConnection()
        {
            var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(email))
            {
                Users[Context.ConnectionId] = email;
                await Clients.All.SendAsync("UserConnected", email);
            }
        }

        // Send message to all clients
        public async Task SendMessageByUser(string message)
        {
            var senderEmail = Users.ContainsKey(Context.ConnectionId) ? Users[Context.ConnectionId] : "Unknown";
            //    await Clients.All.SendAsync("ReceiveMessage", senderEmail, message);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", message);
            var key = Users.FirstOrDefault(u => u.Value == "anas@admin.com").Key;
            Console.Write("key"+key);
            await Clients.Clients(key).SendAsync("ReceiveMessageAtAdmin", message);
        }

        // Send private message to a specific user
        public async Task SendMessageToUser(string message)
        {
           
        }

        // Handle user disconnection
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (Users.TryRemove(Context.ConnectionId, out var email))
            {
                await Clients.All.SendAsync("UserDisconnected", email);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}

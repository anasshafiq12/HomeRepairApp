using HouseRepairApp.Data;
using HouseRepairApp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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

        public async Task SetConnection()
        {
            var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(email))
            {
                Users[Context.ConnectionId] = email;
            }
        }
        public async Task ConfirmBooking(Booking booking)
        {
            var connectionID = Users.FirstOrDefault(u => u.Value == "admin").Key;
            if (connectionID != null)
                await Clients.Client(connectionID).SendAsync("ReceiveBooking", booking);
        }
        public async Task SendMessageToUser(string email, string message)
        {
            var senderUsername = Users[Context.ConnectionId];
            if (senderUsername == "admin")
                senderUsername = "Technician";
            var connectionId = Users.FirstOrDefault(u => u.Value == email).Key;
            if (connectionId != null)
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderUsername, message);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Users.TryRemove(Context.ConnectionId, out _);
            base.OnDisconnectedAsync(exception);
        }
    }
}



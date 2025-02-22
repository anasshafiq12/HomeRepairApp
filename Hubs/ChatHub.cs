using HouseRepairApp.Data.Migrations;
using HouseRepairApp.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace HouseRepairApp.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> Users
            = new ConcurrentDictionary<string, string>();

        // Store messages keyed by sender's email
        
        private static ConcurrentDictionary<string, List<Message>> Messages
            = new ConcurrentDictionary<string, List<Message>>();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatHub(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

      
        // 1. Safely set the connection with null checks
        public async Task SetConnection()
        {
            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext == null)
            {
                // No HttpContext available (could happen if user is not actually in a real HTTP context)
                return;
            }

            var user = httpContext.User;
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                // User not authenticated
                return;
            }

            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                // No email claim found
                return;
            }

            // Register the user’s email to this connection ID
            Users[Context.ConnectionId] = email;
            await Clients.All.SendAsync("UserConnected", email);
        }

       
        // 2. Safely send a message from a normal user to admin
        public async Task SendMessageByUser(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                // Nothing to send
                return;
            }

            // Find who the sender is
            var senderEmail = Users.ContainsKey(Context.ConnectionId)
                ? Users[Context.ConnectionId]
                : "Unknown";

            // Echo the message back to the same client
            await Clients.Client(Context.ConnectionId).SendAsync("MySentMessage", message);

            // Find admin’s connection
            var adminConnectionId = Users.FirstOrDefault(u => u.Value == "anas@admin.com").Key;
            if (!string.IsNullOrEmpty(adminConnectionId))
            {
                await Clients.Client(adminConnectionId).SendAsync("ReceiveMessageAtAdmin", message);
            }

            // Add or update the conversation in the Messages dictionary
            MessageDictionary.Messagedictionary.AddOrUpdate(
                senderEmail,
                // If key does not exist, initialize a new conversation list
                new List<Message>
                {
                    new Message
                    {
                        SenderEmail = senderEmail,
                        ReceiverEmail = "anas@admin.com",
                        MessagesText = new List<string> { message }
                    }
                },
                (existingKey, existingList) =>
                {
                    

                    lock (existingList)
                    {
                        var existingMessage = existingList
                            .FirstOrDefault(m => m.SenderEmail == senderEmail);

                        if (existingMessage != null)
                        {
                            var updatedTexts = existingMessage.MessagesText?.ToList() ?? new List<string>();
                            updatedTexts.Add(message);
                            existingMessage.MessagesText = updatedTexts;
                        }
                        else
                        {
                            existingList.Add(new Message
                            {
                                SenderEmail = senderEmail,
                                ReceiverEmail = "anas@admin.com",
                                MessagesText = new List<string> { message }
                            });
                        }
                    }
                    return existingList;
                }
            );
        }

        // 3. Safely send a private message from admin to a specific user
        public async Task SendMessageToUser(string clientEmail, string message)
        {
            if (string.IsNullOrWhiteSpace(clientEmail) || string.IsNullOrWhiteSpace(message))
            {
                // Nothing to send
                return;
            }

            // Echo back to admin (current connection)
          //  await Clients.Client(Context.ConnectionId).SendAsync("MySentMessage", message);

            // Look up the client’s connection ID by their email
            var clientConnectionId = Users.FirstOrDefault(u => u.Value == clientEmail).Key;
            if (!string.IsNullOrEmpty(clientConnectionId))
            {
                await Clients.Client(clientConnectionId).SendAsync("ReceiveMessage", message);
            }

            // Update the dictionary for admin’s messages
            MessageDictionary.Messagedictionary.AddOrUpdate(
                "anas@admin.com",
                new List<Message>
                {
                    new Message
                    {
                        SenderEmail = "anas@admin.com",
                        ReceiverEmail = clientEmail,
                        MessagesText = new List<string> { message }
                    }
                },
                (existingKey, existingList) =>
                {
                    if (existingList == null)
                        existingList = new List<Message>();

                    lock (existingList)
                    {
                        var existingConversation = existingList
                            .FirstOrDefault(m => m.ReceiverEmail == clientEmail);
                        if (existingConversation != null)
                        {
                            var updatedTexts = existingConversation.MessagesText?.ToList() ?? new List<string>();
                            updatedTexts.Add(message);
                            existingConversation.MessagesText = updatedTexts;
                        }
                        else
                        {
                            existingList.Add(new Message
                            {
                                SenderEmail = "anas@admin.com",
                                ReceiverEmail = clientEmail,
                                MessagesText = new List<string> { message }
                            });
                        }
                    }
                    return existingList;
                }
            );

        }

        // 4. Handle user disconnection
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

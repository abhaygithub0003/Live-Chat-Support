using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Users
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string fromUser, string toUser, string message)
        {
            string groupName = GetGroupName(fromUser, toUser);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", fromUser, message);
        }

        public async Task JoinGroup(string fromUser, string toUser)
        {
            string groupName = GetGroupName(fromUser, toUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string fromUser, string toUser)
        {
            string groupName = GetGroupName(fromUser, toUser);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public string GetGroupName(string user1, string user2)
        {
            return string.CompareOrdinal(user1, user2) < 0 ? $"{user1}_{user2}" : $"{user2}_{user1}";
        }
    }
}

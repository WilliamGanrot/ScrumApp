using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Hubs
{
    public class BoardHub : Hub
    {
        public Task JoinGroup(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
        public Task LeaveGroup(string group)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }

        public Task SendMessageToGroup(string group, string userName)
        {
            System.Diagnostics.Debug.WriteLine(userName);
            return Clients.Group(group).SendAsync("ReceiveMessage", group, userName);
        }
    }
}

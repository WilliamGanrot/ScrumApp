using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Hubs
{
    public class BoardHub : Hub
    {
        public async Task PushNotification(string user, string message)
        {
            //Should not be all. only project members
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

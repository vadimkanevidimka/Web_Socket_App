using Koshelekpy_Test.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using System.Net.WebSockets;

namespace Koshelekpy_Test.Application
{
    public class MessageHub : Hub
    {
        public async Task Send(Message message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message.ToString());
        }
    }
}
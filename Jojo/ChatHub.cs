using Microsoft.AspNetCore.SignalR;

namespace SignalRApp
{
    public class ChatHub : Hub
    {
        public async Task Send(string message, string username)
        {
            var time = DateTime.Now.ToString("HH:mm:ss");
            await Clients.All.SendAsync("Receive", message, username, time);
        }
    }
}

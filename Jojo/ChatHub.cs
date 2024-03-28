using Microsoft.AspNetCore.SignalR;
using Jojo.Models;
using Microsoft.EntityFrameworkCore;

namespace SignalRApp
{

    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task Send(string message, string username, int chatId)
        {
            var chat = await _context.Chats.FindAsync(chatId);

            if (chat != null)
            {
                var time = DateTime.Now.ToUniversalTime();

                _context.ChatMessages.Add(new ChatMessage { Message = message, Username = username, Time = time, ChatId = chatId });

                await _context.SaveChangesAsync();

                await Clients.All.SendAsync("Receive", message, username, time.ToString("HH:mm"));
            }

        }
        public async Task GetChatMessages(int chatId)
        {
            var messages = await _context.ChatMessages.Where(m => m.ChatId == chatId).ToListAsync();
            foreach (var message in messages)
            {
                await Clients.Caller.SendAsync("Receive", message.Message, message.Username, message.Time.ToString("HH:mm"));
            }
        }


    }

}

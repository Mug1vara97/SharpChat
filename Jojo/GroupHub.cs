//
 /*
using Microsoft.AspNetCore.SignalR;
using Jojo.Models;
using Microsoft.EntityFrameworkCore;


namespace SignalRApp
{

    public class ChatGroupHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatGroupHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendGroupMessage(string message, string username, int groupChatId)
        {
            var groupChat = await _context.GroupChats.FindAsync(groupChatId);

            if (groupChat != null)
            {
                var time = DateTime.Now.ToUniversalTime();

                _context.GroupChatMessages.Add(new GroupChatMessage { Message = message, Username = username, Time = time, GroupChatId = groupChatId });

                await _context.SaveChangesAsync();

                await Clients.Group(groupChatId.ToString()).SendAsync("ReceiveGroupMessage", message, username, time.ToUniversalTime().ToString("o"));
            }
        }

        public async Task GetGroupChatMessages(int groupChatId)
        {
            var messages = await _context.GroupChatMessages.Where(m => m.GroupChatId == groupChatId).ToListAsync();
            foreach (var message in messages)
            {
                await Clients.Caller.SendAsync("ReceiveGroupMessage", message.Message, message.Username, message.Time.ToUniversalTime().ToString("o"));
            }
        }

        public async Task JoinGroupChat(int groupChatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupChatId.ToString());
        }
    }


}
 */
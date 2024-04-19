using Microsoft.AspNetCore.SignalR;
using Jojo.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
                var encryptedMessage = Encrypt(message);

                _context.ChatMessages.Add(new ChatMessage { Message = encryptedMessage, Username = username, Time = time, ChatId = chatId });

                await _context.SaveChangesAsync();

                var decryptedMessage = Decrypt(encryptedMessage);
                await Clients.Group(chatId.ToString()).SendAsync("Receive", decryptedMessage, username, time.ToUniversalTime().ToString("o"));
            }
        }


        public async Task GetChatMessages(int chatId)
        {
            var messages = await _context.ChatMessages.Where(m => m.ChatId == chatId).ToListAsync();
            foreach (var message in messages)
            {
                var decryptedMessage = Decrypt(message.Message);
                await Clients.Caller.SendAsync("Receive", decryptedMessage, message.Username, message.Time.ToUniversalTime().ToString("o"));
            }
        }

        public async Task JoinChatGroup(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public string Encrypt(string plainText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes("1234567890123456");
            aes.IV = Encoding.UTF8.GetBytes("1234567890123456");

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            byte[] encrypted;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    encrypted = ms.ToArray();
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string encryptedText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes("1234567890123456");
            aes.IV = Encoding.UTF8.GetBytes("1234567890123456");

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            byte[] encrypted = Convert.FromBase64String(encryptedText);
            string decrypted;

            using (MemoryStream ms = new MemoryStream(encrypted))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        decrypted = sr.ReadToEnd();
                    }
                }
            }

            return decrypted;
        }

        public async Task SendPhoto(string photoData, string username, int chatId)
        {
            var chat = await _context.Chats.FindAsync(chatId);

            if (chat != null)
            {
                var time = DateTime.Now.ToUniversalTime();

                _context.ChatPhotos.Add(new ChatPhoto { PhotoData = photoData, Username = username, Time = time, ChatId = chatId });

                await _context.SaveChangesAsync();

                await Clients.Group(chatId.ToString()).SendAsync("ReceivePhoto", photoData, username, time.ToUniversalTime().ToString("o"));
            }
        }

        public async Task GetChatPhotos(int chatId)
        {
            var photos = await _context.ChatPhotos.Where(p => p.ChatId == chatId).ToListAsync();
            foreach (var photo in photos)
            {
                await Clients.Caller.SendAsync("ReceivePhoto", photo.PhotoData, photo.Username, photo.Time.ToUniversalTime().ToString("o"));
            }
        }
    }
}

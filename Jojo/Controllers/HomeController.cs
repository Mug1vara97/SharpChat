using Jojo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace Jojo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ChatList(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            ViewBag.Username = username;

            var chats = _context.Chats.ToList();
            ViewBag.Chats = chats;

            return View();
        }

        [HttpGet]
        public IActionResult Chat(string username, int chatId, string chatName)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            ViewBag.Username = username;
            ViewBag.ChatId = chatId;

            var chat = _context.Chats.Find(chatId);
            if (chat != null)
            {
                ViewBag.ChatName = chat.Name;
            }

            var messages = _context.ChatMessages.Where(m => m.ChatId == chatId).ToList();
            ViewBag.Messages = messages;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateChat(string chatName, string chatDescription, string username)
        {
            if (!string.IsNullOrEmpty(chatName) && !string.IsNullOrEmpty(username))
            {
                var newChat = new Chat { Name = chatName, Description = chatDescription, CreatedBy = username };
                _context.Chats.Add(newChat);
                await _context.SaveChangesAsync(); _context.SaveChanges();
            }

            return RedirectToAction("ChatList", new { username });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteChat(string username, int chatId)
        {
            var chat = _context.Chats.Find(chatId);

            if (chat != null)
            {
                if (chat.CreatedBy == username)
                {
                    _context.Chats.Remove(chat);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("ChatList", new { username });
        }

        public IActionResult DeleteChat()
        {
            return View();
        }

        public IActionResult CreateChat()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
       public async Task<IActionResult> Register(User user, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(user);
                }

                if (user.Password == confirmPassword)
                {
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                        user.Password = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                    }
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("confirmPassword", "Passwords do not match");
                }
            }

            return View(user);
        }

        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                    if (user.Password == hashedPassword)
                    {
                        return RedirectToAction("ChatList", new { username = user.Username });
                    }
                }
            }
            return View();
        }

    }
}
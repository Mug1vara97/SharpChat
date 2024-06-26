using Jojo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

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
            var userChats = chats.Where(c => c.AllowedUsers.Contains(username) || c.AllowedUsers.Contains("public")).ToList();

            ViewBag.Chats = userChats;

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
                if (chat.AllowedUsers.Contains(username) || chat.AllowedUsers.Contains("public"))
                {
                    ViewBag.ChatName = chat.Name;

                    var messages = _context.ChatMessages.Where(m => m.ChatId == chatId).ToList();
                    ViewBag.Messages = messages;

                    return View();
                }
            }

            return RedirectToAction("ChatList", new { username });
        }


        [HttpPost]
        public async Task<IActionResult> CreateChat(string chatName, string chatDescription, string username, string addUsersType, string allowedUsers)
        {
            if (!string.IsNullOrEmpty(chatName) && !string.IsNullOrEmpty(username))
            {
                List<string> allowedUsersList = new List<string>();

                if (addUsersType == "all")
                {
                    allowedUsersList.Add("public");
                }
                else
                {
                    allowedUsersList = allowedUsers.Split(',').Select(u => u.Trim()).ToList();
                }

                if (!allowedUsersList.Contains(username))
                {
                    allowedUsersList.Add(username);
                }

                var newChat = new Chat { Name = chatName, Description = chatDescription, CreatedBy = username, AllowedUsers = allowedUsersList };
                _context.Chats.Add(newChat);
                await _context.SaveChangesAsync();
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

                    user.BackgroundColor = "#007bff";

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
                        return RedirectToAction("NewsFeed", new { username = user.Username });
                    }
                }
            }
            return View();
        }

        public IActionResult UsersList(string username)
        {
            ViewBag.Username = username;
            var users = _context.Users.OrderBy(u => u.Username).ToList();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToChat(string username, int chatId, string addedUser)
        {
            var chat = _context.Chats.Find(chatId);

            if (chat != null && chat.CreatedBy == username)
            {
                chat.AllowedUsers.Add(addedUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ChatList", new { username });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromChat(string username, int chatId, string removedUser)
        {
            var chat = _context.Chats.Find(chatId);

            if (chat != null && chat.CreatedBy == username)
            {
                chat.AllowedUsers.Remove(removedUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ChatList", new { username });
        }
        public async Task<IActionResult> NewsFeed(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            var newsFeedItems = await _context.NewsFeedItems.Include(n => n.Comments).Include(n => n.Likes).ToListAsync();


            ViewBag.Username = username;
            var chats = _context.Chats.ToList();
            var userChats = chats.Where(c => c.AllowedUsers.Contains(username) || c.AllowedUsers.Contains("public")).ToList();

            ViewBag.Chats = userChats;

            return View(newsFeedItems);
        }

        public IActionResult CreateNews(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews(string username, string title, string content, IFormFile photo)
        {
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content))
            {
                var newNewsFeedItem = new NewsFeedItem
                {
                    Title = title,
                    Content = content,
                    PostedDate = DateTime.UtcNow,
                    Author = username
                };

                if (photo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photo.CopyToAsync(memoryStream);
                        newNewsFeedItem.Photo = memoryStream.ToArray();
                        newNewsFeedItem.ContentType = photo.ContentType;
                    }
                }

                _context.NewsFeedItems.Add(newNewsFeedItem);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteNews(int newsItemId)
        {
            var newsItem = _context.NewsFeedItems.Find(newsItemId);

            if (newsItem != null)
            {
                _context.NewsFeedItems.Remove(newsItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("NewsFeed");
        }
        public IActionResult GetNews(int id)
        {
            var newsItem = _context.NewsFeedItems.Find(id);
            return Json(newsItem);
        }

        [HttpPost]
        public async Task<IActionResult> EditNews(int newsId, string title, string content)
        {
            var newsItem = _context.NewsFeedItems.Find(newsId);

            if (newsItem != null)
            {
                newsItem.Title = title;
                newsItem.Content = content;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
        public IActionResult Profile(string username, string profilename)
        {
            ViewBag.Username = username;

            var user = _context.Users.FirstOrDefault(u => u.Username == profilename);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.ViewingUser = profilename;
            var userNewsItems = _context.NewsFeedItems.Where(n => n.Author == profilename).ToList();

            ViewBag.UserNewsItems = userNewsItems;

            var userFriends = _context.Friends.Where(f => (f.User1 == profilename || f.User2 == profilename)).ToList();
            ViewBag.Friends = userFriends;

            var userIsFriend = _context.Friends.Any(f => (f.User1 == username && f.User2 == profilename) || (f.User1 == profilename && f.User2 == username));
            ViewBag.IsFriend = userIsFriend;
            ViewBag.FriendshipRequests = _context.FriendshipRequests.ToList();



            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(string username, string profileDescription, IFormFile profilePhoto, string backgroundColor)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                user.ProfileDescription = profileDescription;

                if (profilePhoto != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await profilePhoto.CopyToAsync(memoryStream);
                        user.ProfilePhoto = memoryStream.ToArray();
                        user.ProfilePhotoContentType = profilePhoto.ContentType;
                    }
                }
                user.BackgroundColor = backgroundColor;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Profile", new { username = username, profilename = username });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int newsItemId, string username, string content)
        {
            var newsItem = _context.NewsFeedItems.Include(n => n.Comments).FirstOrDefault(n => n.Id == newsItemId);

            if (newsItem != null)
            {
                var comment = new Comment
                {
                    NewsFeedItemId = newsItemId,
                    Author = username,
                    Content = content,
                    PostedDate = DateTime.UtcNow
                };

                newsItem.Comments.Add(comment);

                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(string username, string friendUsername)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(friendUsername))
            {
                return BadRequest();
            }

            var friendshipExists = _context.Friends.Any(f => (f.User1 == username && f.User2 == friendUsername) || (f.User1 == friendUsername && f.User2 == username));
            if (friendshipExists)
            {
                return BadRequest("Friendship already exists");
            }

            var newFriendship = new Friendship { User1 = username, User2 = friendUsername };
            _context.Friends.Add(newFriendship);
            await _context.SaveChangesAsync();

            return RedirectToAction("Profile", new { username = username, profilename = friendUsername });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriend(string username, string friendUsername)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(friendUsername))
            {
                return BadRequest();
            }

            var friendship = _context.Friends.FirstOrDefault(f =>
                (f.User1 == username && f.User2 == friendUsername) || (f.User1 == friendUsername && f.User2 == username));

            if (friendship != null)
            {
                _context.Friends.Remove(friendship);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Profile", new { username = username, profilename = friendUsername });
        }

        [HttpPost]
        public async Task<IActionResult> LikeNews(int newsItemId, string username)
        {
            var existingLike = _context.Likes.FirstOrDefault(l => l.NewsFeedItemId == newsItemId && l.Username == username);

            if (existingLike == null)
            {
                var newLike = new Like { NewsFeedItemId = newsItemId, Username = username };
                _context.Likes.Add(newLike);
            }
            else
            {
                _context.Likes.Remove(existingLike);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UnlikeNews(int newsItemId, string username)
        {
            var existingLike = _context.Likes.FirstOrDefault(l => l.NewsFeedItemId == newsItemId && l.Username == username);

            if (existingLike != null)
            {
                _context.Likes.Remove(existingLike);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(string fromUser, string toUser)
        {
            var existingRequest = _context.FriendshipRequests.FirstOrDefault(fr => fr.FromUser == fromUser && fr.ToUser == toUser);

            if (existingRequest == null)
            {
                var newRequest = new FriendshipRequest { FromUser = fromUser, ToUser = toUser, IsAccepted = false };
                _context.FriendshipRequests.Add(newRequest);
                await _context.SaveChangesAsync();

                return Ok("Friend request sent");
            }

            return BadRequest("Friend request already sent");
        }


        [HttpPost]
        public async Task<IActionResult> AcceptFriendRequest(int requestId)
        {
            var request = _context.FriendshipRequests.Find(requestId);

            if (request != null)
            {
                request.IsAccepted = true;
                _context.FriendshipRequests.Remove(request);

                var newFriendship = new Friendship { User1 = request.FromUser, User2 = request.ToUser };
                _context.Friends.Add(newFriendship);

                await _context.SaveChangesAsync();

                return Ok("Friend request accepted");
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> RejectFriendRequest(int requestId)
        {
            var request = _context.FriendshipRequests.Find(requestId);

            if (request != null)
            {
                _context.FriendshipRequests.Remove(request);
                await _context.SaveChangesAsync();

                return Ok("Friend request rejected");
            }

            return NotFound();
        }
        public IActionResult Snake(string username)
        {
            ViewBag.Username = username;
            var topPlayers = _context.SnakeGameStats.OrderByDescending(s => s.Score).Take(10).ToList(); // �������� ��� 10 ������� �� �����

            var uniquePlayers = topPlayers.GroupBy(p => p.Username).Select(p => p.First()).ToList(); // �������� ������ ���������� ������� �� �����

            return View(uniquePlayers);
        }

        [HttpPost]
        public IActionResult SaveSnakeGameStats([FromBody] SnakeGameStats stats)
        {
            try
            {
                _context.SnakeGameStats.Add(stats);
                _context.SaveChanges();
                return Json(new { success = true, message = "Snake game stats saved successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
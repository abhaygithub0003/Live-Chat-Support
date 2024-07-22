using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Users.Data;
using Users.Models;

namespace Users.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IHubContext<ChatHub> hubContext, ApplicationDbContext context)
        {
            _logger = logger;
            _hubContext = hubContext;
            _context = context;
        }

        public IActionResult Index(string userEmail = null, string errorMessage = null)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var chatUser = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (chatUser == null && userEmail != null)
            {
                return RedirectToAction("Index", new { errorMessage = "User not found." });
            }

            ViewBag.ErrorMessage = errorMessage;
            ViewBag.ChatUser = chatUser;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string message, string toUserEmail)
        {
            var fromUser = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (fromUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var toUser = _context.Users.FirstOrDefault(u => u.Email == toUserEmail);
            if (toUser == null)
            {
                return RedirectToAction("Index", new { errorMessage = "User not found." });
            }

            string groupName = GetGroupName(fromUser.Email, toUser.Email);
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", fromUser.Email, message);
            return RedirectToAction("Index", new { userEmail = toUser.Email });
        }

        private string GetGroupName(string user1, string user2)
        {
            return string.CompareOrdinal(user1, user2) < 0 ? $"{user1}_{user2}" : $"{user2}_{user1}";
        }
    }
}

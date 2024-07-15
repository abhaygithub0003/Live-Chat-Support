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
        public IActionResult Index(string errorMessage = null)
        {           
            var currentUser = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = errorMessage;            
            int userId = currentUser.Id;
            return View(); 
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
            return RedirectToAction("Index");
        }
    }
}

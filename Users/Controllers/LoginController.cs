﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Users.Data;
using Users.Models;
using Users.Models.ViewModels;

namespace Users.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        public LoginController(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewodel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email)
                        };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow + TimeSpan.FromMinutes(30)
                    });

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid email or password. Please try again.");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User users)
        {
            if (users == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailExists = _context.Users.FirstOrDefault(u => u.Email == users.Email);
            if (emailExists != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(new User());
            }

            string hashPass = BCrypt.Net.BCrypt.HashPassword(users.Password);
            users.Password = hashPass;

            _context.Users.Add(users);
            _context.SaveChanges();

            var userRole = new UserRoles
            {
                UserId = users.Id,
                Role = _context.Users.Count() == 1 ? "SupportAgent" : "User"
            };
            _context.UserRoles.Add(userRole);

            _context.SaveChanges();

            if (userRole.Role == "User")
            {
                var supportAgent = _context.Users.FirstOrDefault(u => _context.UserRoles.Any(r => r.UserId == u.Id && r.Role == "SupportAgent"));
                if (supportAgent != null)
                {
                    // Create group name using user and support agent IDs or emails
                    string groupName = $"Group_{users.Id}_{supportAgent.Id}";

                    // Notify SignalR to join group
                    await _hubContext.Clients.User(users.Email).SendAsync("JoinGroup", groupName);
                    await _hubContext.Clients.User(supportAgent.Email).SendAsync("JoinGroup", groupName);
                }
            }

            return RedirectToAction("Index", "Login");
        }


        [HttpGet("/api/GetCurrentUserName")]
        public IActionResult GetCurrentUserName()
        {
            var currentUser = HttpContext.User;
            if (currentUser == null || !currentUser.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userEmail = currentUser.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.FirstName); 
        }
        [HttpGet("/api/GetCurrentUserId")]
        public IActionResult GetCurrentUserId()
        {
            var currentUser = HttpContext.User;
            if (currentUser == null || !currentUser.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userEmail = currentUser.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.Id);
        }


    }
}

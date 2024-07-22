using Microsoft.AspNetCore.Mvc;
using Users.Data;
using Users.Models;
using System.Linq;

namespace Users.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
    }
}

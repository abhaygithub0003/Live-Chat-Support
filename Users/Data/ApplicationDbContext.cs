using Microsoft.EntityFrameworkCore;
using Users.Models;

namespace Users.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }

}
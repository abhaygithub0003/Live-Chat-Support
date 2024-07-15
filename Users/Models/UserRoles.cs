namespace Users.Models
{
    public class UserRoles
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

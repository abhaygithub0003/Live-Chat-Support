namespace Users.Models.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public UserRoles Roles { get; set; }

        public IEnumerable<UserRoles> UserRoles { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace Users.Models.ViewModels
{
    public class LoginViewodel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }    

    }
}

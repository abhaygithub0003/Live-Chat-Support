using System.ComponentModel.DataAnnotations;

namespace Users.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
        public string City { get; set; }
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        [Required]
        public string Password { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}

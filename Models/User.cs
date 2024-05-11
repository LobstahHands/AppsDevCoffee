using System.ComponentModel.DataAnnotations;

namespace AppsDevCoffee.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Maximum 30 characters")]
        [RegularExpression(@"^.{3,}$", ErrorMessage = "Minimum 3 characters required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Maximum 30 characters")]
        [RegularExpression(@"^.{3,}$", ErrorMessage = "Minimum 3 characters required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        public int UserTypeId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        public string? Hashed { get; set; }  // Should this be nullable?

        public string UserStatus { get; set; }

        public DateTime DateAdded { get; set; }

        // Navigation property
        public UserType UserType { get; set; }

        // Navigation property
        public List<Order> Orders { get; set; }
    }
}

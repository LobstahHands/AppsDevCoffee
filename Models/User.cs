using AppsDevCoffee.Models.TypeTables;
using System.ComponentModel.DataAnnotations;

namespace AppsDevCoffee.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
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

using AppsDevCoffee.Models.TypeTables;

namespace AppsDevCoffee.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int UserTypeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salted { get; set; }
        public string Hashed { get; set; }
        public int Active { get; set; }
        public DateTime DateAdded { get; set; }

        // Navigation property
        public UserType UserType { get; set; }

        // Navigation property
        public List<Order> Orders { get; set; }
    }
}

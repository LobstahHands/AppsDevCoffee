namespace AppsDevCoffee.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        /*Salted/Hashed
        public string salted { get; set; }
        public string hashed { get; set; }
        */



    }
}

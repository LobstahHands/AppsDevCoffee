namespace AppsDevCoffee.Models
{
    public static class DB
    {

        public static List<User> userList = [

            new User("Joe", "Kopacz", "Joe@mail.com", "Password1!"),
            new User("Angie", "Kopacz", "Angie@mail.com", "Password1!"),
            new User("Sebastian", "Kopacz", "Seb@mail.com", "Password1!"),
            new User("Lewis", "Kopacz", "Lewis@mail.com", "Password1!")

            ];

        public static List<User> getUsers()
        {
            List<User> users = [];
            users = userList;

            return users;
        }
    }
}
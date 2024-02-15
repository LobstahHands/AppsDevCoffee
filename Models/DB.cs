namespace AppsDevCoffee.Models
{
    public static class DB
    {

        public static List<User> userList = [

            new User("Joe", "Kopacz", "joe@mail.com", "Password1!"),
            new User("Angie", "Kopacz", "angie@mail.com", "Password1!"),
            new User("Sebastian", "Kopacz", "seb@mail.com", "Password1!"),
            new User("Lewis", "Kopacz", "lewis@mail.com", "Password1!")

            ];

        public static List<User> getUsers()
        {
            List<User> users = [];
            users = userList;

            return users;
        }
    }
}
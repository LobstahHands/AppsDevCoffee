using Microsoft.AspNetCore.Mvc;
using AppsDevCoffee.Models;


namespace AppsDevCoffee.Controllers
{
    public class UserController(CoffeeAppContext ctx) : Controller
    {
        private CoffeeAppContext context { get; set; } = ctx;
        public IActionResult Index()
        {
            return View();
        }

        //General User options:
        //New Order (move to order controller?)
        //View Order History



        //Admin Options
        public IActionResult UserList()
        {
            List<User> users = context.Users.ToList();
            return View(users);
        }

        public IActionResult Orderlist()
        {
            return View();
        }

        public IActionResult InventoryList()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        //Login 
        [HttpPost]
        public IActionResult Login(string email, string password) {

            List<User> users = new List<User>();

            User user = users.FirstOrDefault(user => user.Email == email.Trim().ToLower());


            if (user == null) {
                return View("AccessDenied");
            }
            else if (user.Password == password) {
                return View("Index", user);
            }
            else {
                return View("AccessDenied");
            }

             
        }


    }

}

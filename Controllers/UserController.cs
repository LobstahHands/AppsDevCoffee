using Microsoft.AspNetCore.Mvc;
using AppsDevCoffee.Models;


namespace AppsDevCoffee.Controllers
{
    public class UserController : Controller
    {
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
            List<User> users = DB.getUsers();
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

            List<User> users = DB.getUsers();

            User user = users.FirstOrDefault(user => user.Email == email); 
            if (user.Password == password) {
                return View("Index", user);
            }
            else { 
                return View("AccessDenied");   
            }

             
        }


    }

}

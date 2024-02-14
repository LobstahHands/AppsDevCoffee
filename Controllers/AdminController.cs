using Microsoft.AspNetCore.Mvc;
using AppsDevCoffee.Models;


namespace AppsDevCoffee.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

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

    }

}

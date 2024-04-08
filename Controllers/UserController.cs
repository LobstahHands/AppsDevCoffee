using Microsoft.AspNetCore.Mvc;
using AppsDevCoffee.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace AppsDevCoffee
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly CoffeeAppContext Context;

        public AccountController(CoffeeAppContext context)
        {
            Context = context;
        }

        // Show a list of all orders associated with this user
        public IActionResult OrderList()
        {
            var userId = int.Parse(User.Identity.Name); // Assuming the user ID is stored in the Name claim
            var orders = Context.Orders.Include(o => o.User)
                                        .Where(o => o.UserId == userId)
                                        .ToList();
            return View(orders);
        }

        // Show all the details of a specific order
        public IActionResult OrderDetail(int id)
        {
            var order = Context.Orders.Include(o => o.User)
                                       .FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // A new order view to place an order
        [HttpGet]
        public IActionResult NewOrder()
        {
            // Add logic to populate dropdowns or any other data required for the new order form
            return View();
        }

        // The GET method brings them to the edit view to edit their user details
        [HttpGet]
        public IActionResult Edit()
        {
            var userId = int.Parse(User.Identity.Name); // Assuming the user ID is stored in the Name claim
            var user = Context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // The POST method updates the database
        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                Context.Users.Update(user);
                Context.SaveChanges();
                return RedirectToAction("Index", "Home"); // Redirect to a suitable action after successful edit
            }
            return View(user);
        }
    }
}
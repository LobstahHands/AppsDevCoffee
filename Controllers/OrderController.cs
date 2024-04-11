using AppsDevCoffee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppsDevCoffee.Controllers
{
    [Authorize]
    public class OrderController(CoffeeAppContext ctx) : Controller
    {
        private readonly CoffeeAppContext context = ctx;
      
        public IActionResult Index()
        {
            // Retrieve the user ID from the claims
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                // User ID claim not found, handle accordingly (e.g., redirect to login)
                return RedirectToAction("Login", "Account");
            }

            // Parse the user ID from the claim
            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                // Unable to parse user ID, handle accordingly
                return RedirectToAction("Login", "Account");
            }

            // Retrieve orders for the logged-in user
            var orders = context.Orders.Where(o => o.UserId == userId).ToList();

            // Pass the filtered orders to the view
            return View(orders);
        }

        // GET: /Order/Add
        public IActionResult Add()
        {
            // Populate any necessary data for creating a new order, e.g., users, products, etc.
            Order order = new Order();
            return View("Edit",order);
        }

        // POST: /Order/Add
        [HttpPost]
        [ValidateAntiForgeryToken] //do this for post methods 
        public IActionResult Add(Order order)
        {
            if (ModelState.IsValid)
            {
                // Set any default values or perform any additional validation before saving
                order.OrderDate = DateTime.Now;

                context.Orders.Add(order);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(order);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = context.Orders.Find(id);
            context.Orders.Remove(order);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return context.Orders.Any(o => o.Id == id);
        }

        public IActionResult NewOrder() 
        {
            var availableCoffees = context.OriginTypes.ToList();
            ViewBag.AvailableCoffees = availableCoffees;
            return View();
        }
    }
}

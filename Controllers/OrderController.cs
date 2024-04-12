using AppsDevCoffee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
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
            return View("Edit", order);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var availableOriginTypes = context.OriginTypes.ToList();
            var availableRoastTypes = context.RoastTypes.ToList();

            ViewBag.AvailableOriginTypes = availableOriginTypes;
            ViewBag.AvailableRoastTypes = availableRoastTypes;

            return View();
        }


        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string OriginTypeId, int RoastTypeId, string OzQuantity, string submitType)
        {
            var userId = GetCurrentUserId();
            var orderDate = DateTime.Now;

            // Create a new order object
            var order = new Order
            {
                UserId = int.Parse(userId),
                OrderDate = orderDate,
                PriceAdjustment = 0,
                SubtotalCost = 0,
                TotalCost = 0
            };

            if (submitType == "addToOrder")
            {
                // Format the selected order item
                var coffee = context.OriginTypes.Find(int.Parse(OriginTypeId));
                var roastType = RoastTypeId;
                var ozQuantity = int.Parse(OzQuantity);
                var orderItemString = $"{coffee.Country} - {coffee.SupplierNotes}, Roast Type: {roastType}, Oz Quantity: {ozQuantity}";

                // Add the selected order item to TempData
                var orderItems = TempData.ContainsKey("orderItems") ? TempData["orderItems"] as List<string> : new List<string>();
                orderItems.Add(orderItemString);
                TempData["orderItems"] = orderItems;

                return RedirectToAction("Create");
            }
            else if (submitType == "createOrder")
            {
                // Handle creating the order
                // Add the order to the database
                order.TotalCost = order.PriceAdjustment + order.SubtotalCost;
                context.Orders.Add(order);
                context.SaveChanges();

                return RedirectToAction("OrderDetail", "Order", order.Id);
            }
            else
            {
                // Invalid submit type
                return RedirectToAction("Create", "Order");
            }
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
        public IActionResult OrderDetail(int id)
        {
            var order = context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.OriginType).FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
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
        [ValidateAntiForgeryToken]
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

        public string GetCurrentUserId()
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            // Check if the claim exists and return its value
            if (userIdClaim != null)
            {
                return userIdClaim.Value;
            }
            else
            {
                // If the claim does not exist, redirect the user to the login action
                return RedirectToAction("Login", "Account").ToString(); // Assuming Account is the name of your account controller
            }
        }
    }
}

﻿using AppsDevCoffee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AppsDevCoffee.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly CoffeeAppContext context;

        public OrderController(CoffeeAppContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            // Retrieve the user type ID from the claims
            var userTypeIdClaim = HttpContext.User.FindFirst("UserTypeId");
            if (userTypeIdClaim == null)
            {
                // User type ID claim not found, handle accordingly
                return RedirectToAction("Logout", "Account");
            }

            // Parse the user type ID from the claim
            if (!int.TryParse(userTypeIdClaim.Value, out int userTypeId))
            {
                // Unable to parse user type ID, handle accordingly
                return RedirectToAction("Logout", "Account");
            }

            List<Order> orders = new List<Order>();
            // Check if the user type is admin (User Type ID = 1)
            if (userTypeId == 1)
            {
                // Retrieve all orders since the user is admin
                orders = context.Orders.ToList();

            }
            else
            {
                // Retrieve orders for the logged-in user
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    // User ID claim not found, handle accordingly
                    return RedirectToAction("Login", "Account");
                }

                // Parse the user ID from the claim
                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    // Unable to parse user ID, handle accordingly
                    return RedirectToAction("Logout", "Account");
                }

                // Retrieve orders for the logged-in user
                orders = context.Orders.Where(o => o.UserId == userId).ToList();

            }
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
            SetViewTypes();

            // Initialize OrderItems if null
            var createItemViewModel = new CreateItemViewModel
            {
                OrderItems = HttpContext.Session.Get<List<OrderItem>>("OrderItems") ?? []
            };

            return View(createItemViewModel);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateItem(CreateItemViewModel createItemViewModel)
        {
            ModelState.Remove("OrderItems");
            SetViewTypes();
            if (ModelState.IsValid)
            {
                int orderTypeIdConverted = (int)createItemViewModel.OriginTypeId;
                var orderItem = new OrderItem
                {
                    OriginTypeId = orderTypeIdConverted,
                    OriginType = context.OriginTypes.Find(orderTypeIdConverted),
                    RoastTypeId = createItemViewModel.RoastTypeId,
                    OzQuantity = createItemViewModel.OzQuantity
                };

                orderItem.CalculateSubtotal();

                // get order items from session
                var orderItems = HttpContext.Session.Get<List<OrderItem>>("OrderItems") ?? new List<OrderItem>();
                orderItems.Add(orderItem);

                // put order items in session
                HttpContext.Session.Set("OrderItems", orderItems);

                // Pass order items to the view
                createItemViewModel.OrderItems = orderItems;
            }

            // Handle invalid model state
            return View("Create", createItemViewModel);
        }


        [HttpPost]
        public IActionResult RemoveOrderItem(int originTypeId, int roastTypeId, int ozQuantity)
        {
            // Retrieve the list of order items from session
            var orderItems = HttpContext.Session.Get<List<OrderItem>>("OrderItems");
            string action = "Create";

            if (orderItems != null)
            {
                
                    // Find and remove the specific order item
                    var orderItemToRemove = orderItems
                        .FirstOrDefault(item => item.OriginTypeId == originTypeId && item.OzQuantity == ozQuantity && item.RoastTypeId == roastTypeId);

                    if (orderItemToRemove != null)
                    {
                        orderItems.Remove(orderItemToRemove);
                    }

                // Update the order items in session
                HttpContext.Session.Set("OrderItems", orderItems);
            }

            // Redirect back to the same page or another appropriate action
            return RedirectToAction(action);
        }


        [HttpPost]
        public IActionResult CreateOrder()
        {
            var userId = GetCurrentUserId();

            //handle the error. 
            if (userId == 0) { return RedirectToAction("Index", "Home");}

            // Create a new order object
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                PriceAdjustment = 1,
                TotalPaid = 0,
                OrderStatus = "Awaiting Roaster"
            };

            // Retrieve order items from session
            var orderItems = HttpContext.Session.Get<List<OrderItem>>("OrderItems");

            if (orderItems != null && orderItems.Any())
            {
                order.SubtotalCost = orderItems.Sum(item => item.Subtotal);
                HttpContext.Session.Remove("OrderItems");

                //Add the order to the DB and get an id.
                order.TotalCost = order.PriceAdjustment * order.SubtotalCost;
                context.Orders.Add(order);

                context.SaveChanges();


                // Add order items to the database
                foreach (var item in orderItems)
                {
                    item.Order = order; //include connection to order
                    item.OriginType = null; //duplicating origintype and origintypeid threw an error. 
                    context.OrderItems.Add(item);
                }
                // Clear order items from session
                context.SaveChanges();


            }
            else
            {
                return RedirectToAction("Index","Home");
            }

            

            return RedirectToAction("OrderPayment", new { id = order.Id });
        }

        // GET: OrderPayment
        public IActionResult OrderPayment(int id)
        {
            var order = context.Orders.FirstOrDefault(o => o.Id == id);

            // Check if the ViewBag properties are null
            if (order.TotalCost == null || order.Id == null)
            {
                // Redirect
                return RedirectToAction("Index");
            }

            // Pass ViewBag properties to the view
            ViewBag.TotalCost = order.TotalCost;
            ViewBag.OrderId = order.Id;

            // Return the OrderPayment view
            return View();
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
        public IActionResult Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            ModelState.Remove("User");
            ModelState.Remove("OrderItems");
            ModelState.Remove("OrderStatus");
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
        public IActionResult Delete(int id) 
        {
            var order = context.Orders.Find(id);
            return View(order);
        }

        
        // POST: Order/Delete/{id}
        [HttpPost] //, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, bool confirm)
        {
            var order = context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderItems = context.OrderItems.Where(o => o.OrderId == order.Id);
            
            context.OrderItems.RemoveRange(orderItems);
            context.Orders.Remove(order);
            context.SaveChanges();
            return RedirectToAction("Index", "Order");
        }
        


        private bool OrderExists(int id)
        {
            return context.Orders.Any(o => o.Id == id);
        }

        public int GetCurrentUserId()
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            // Check if the claim exists and return its value
            if (userIdClaim != null)
            {
                return int.Parse(userIdClaim.Value);
            }
            else
            {
                return 0; 
            }
        }


        private void SetViewTypes()
        {
            var availableOriginTypes = context.OriginTypes.Where(o => o.Active == 1).ToList();
            var availableRoastTypes = context.RoastTypes.ToList();

            ViewBag.AvailableOriginTypes = availableOriginTypes;
            ViewBag.AvailableRoastTypes = availableRoastTypes;
        }



    }
}

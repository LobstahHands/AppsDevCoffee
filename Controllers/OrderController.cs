﻿using AppsDevCoffee.Models;
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
                return RedirectToAction("Logout", "Account");
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

                // Retrieve order items from session
                var orderItems = HttpContext.Session.Get<List<OrderItem>>("OrderItems") ?? new List<OrderItem>();
                orderItems.Add(orderItem);

                // Store order items in session
                HttpContext.Session.Set("OrderItems", orderItems);

                // Pass order items to the view
                createItemViewModel.OrderItems = orderItems;
            }

            // Handle invalid model state
            return View("Create", createItemViewModel);
        }


        [HttpPost]
        public IActionResult RemoveOrderItem(int? originTypeId = null, int? ozQuantity = null, int? roastId = null)
        {
            // Retrieve the list of order items from session
            var orderItems = HttpContext.Session.Get<List<OrderItem>>("OrderItems");
            string action = "Create";

            if (orderItems != null)
            {
                if (originTypeId.HasValue && ozQuantity.HasValue && roastId.HasValue)
                {
                    // Find and remove the specific order item
                    var orderItemToRemove = orderItems
                        .FirstOrDefault(item => item.OriginTypeId == originTypeId && item.OzQuantity == ozQuantity && item.RoastTypeId == roastId);

                    if (orderItemToRemove != null)
                    {
                        orderItems.Remove(orderItemToRemove);
                    }
                }
                else
                {
                    // Remove all order items
                    orderItems.Clear();
                    action = "Index"; //Navigate back out

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

            // Create a new order object
            var order = new Order
            {
                UserId = int.Parse(userId),
                OrderDate = DateTime.Now,
                PriceAdjustment = 1,
                OrderStatus = "Pending"
            };

            // Retrieve order items from session
            var orderItems = HttpContext.Session.Get<List<OrderItem>>("OrderItems");

            if (orderItems != null && orderItems.Any())
            {
                order.SubtotalCost = orderItems.Sum(item => item.Subtotal);

                // Clear order items from session
                HttpContext.Session.Remove("OrderItems");
            }
            else
            {
                return RedirectToAction("Index","Home");
            }

            order.TotalCost = order.PriceAdjustment * order.SubtotalCost;
            context.Orders.Add(order);
            context.SaveChanges();

            HttpContext.Session.SetInt32("OrderTotalCost", (int)order.TotalCost);
            HttpContext.Session.SetInt32("OrderId", order.Id);

            return RedirectToAction("OrderPayment");
        }

        // GET: OrderPayment
        public IActionResult OrderPayment()
        {
            // Get ViewBag properties for the view
            var totalCost = HttpContext.Session.GetInt32("OrderTotalCost");
            var orderId = HttpContext.Session.GetInt32("OrderId");

            // Check if the ViewBag properties are null
            if (totalCost == null || orderId == null)
            {
                // Redirect to an error page or handle the situation appropriately
                return RedirectToAction("Error");
            }

            // Pass ViewBag properties to the view
            ViewBag.TotalCost = totalCost;
            ViewBag.OrderId = orderId;

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


        private void SetViewTypes()
        {
            var availableOriginTypes = context.OriginTypes.ToList();
            var availableRoastTypes = context.RoastTypes.ToList();

            ViewBag.AvailableOriginTypes = availableOriginTypes;
            ViewBag.AvailableRoastTypes = availableRoastTypes;
        }



    }
}

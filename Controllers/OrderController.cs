using AppsDevCoffee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppsDevCoffee.Controllers
{
    public class OrderController(CoffeeAppContext ctx) : Controller
    {
        private readonly CoffeeAppContext context = ctx;
        // GET: /Order
        public IActionResult Index()
        {
            var orders = context.Orders.ToList();
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
        [ValidateAntiForgeryToken]
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

        // GET: /Order/Edit/5
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

        // POST: /Order/Edit/5
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

        // GET: /Order/Delete/5
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

        // POST: /Order/Delete/5
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

        public IActionResult NewOrder() 
        {
            List<CurrentInventory> currentInventory = new List<CurrentInventory>();
            return View(currentInventory);
        }
    }
}

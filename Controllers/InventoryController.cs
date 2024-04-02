using AppsDevCoffee.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppsDevCoffee.Controllers
{
    public class CurrentInventoryController(CoffeeAppContext ctx) : Controller
    {
        private readonly CoffeeAppContext context = ctx;

        public IActionResult Index()
        {
            var inventoryItems = context.CurrentInventories.ToList();
            return View(inventoryItems);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItem = context.CurrentInventories.FirstOrDefault(i => i.Id == id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CurrentInventory inventoryItem)
        {
            if (ModelState.IsValid)
            {
                context.CurrentInventories.Add(inventoryItem);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryItem);
        }

    
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItem = context.CurrentInventories.FirstOrDefault(i => i.Id == id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CurrentInventory inventoryItem)
        {
            if (id != inventoryItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                context.Update(inventoryItem);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryItem);
        }

     
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItem = context.CurrentInventories.FirstOrDefault(i => i.Id == id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var inventoryItem = context.CurrentInventories.FirstOrDefault(i => i.Id == id);
            context.CurrentInventories.Remove(inventoryItem);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

using AppsDevCoffee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppsDevCoffee.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class InventoryController : Controller
    {
        private readonly CoffeeAppContext Context;

        public InventoryController(CoffeeAppContext context)
        {
            Context = context;
        }

        // Method to display list of all origin types
        public IActionResult InventoryList()
        {
            var originTypes = Context.OriginTypes.ToList();
            return View(originTypes);
        }

        // GET method to display the update form for a specific origin type
        public IActionResult UpdateInventory(int id)
        {
            var originType = Context.OriginTypes.Find(id);
            if (originType == null)
            {
                return NotFound();
            }
            return View(originType);
        }

        // POST method to update the database with changes to an origin type
        [HttpPost]
        public IActionResult UpdateInventory(int id, OriginType originType)
        {
            if (id != originType.OriginTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Context.Update(originType);
                Context.SaveChanges();
                return RedirectToAction(nameof(InventoryList));
            }
            return View(originType);
        }

        // POST method to delete an origin type
        [HttpPost]
        public IActionResult DeleteInventory(int id)
        {
            var originType = Context.OriginTypes.Find(id);
            if (originType == null)
            {
                return NotFound();
            }

            Context.OriginTypes.Remove(originType);
            Context.SaveChanges();
            return RedirectToAction(nameof(InventoryList));
        }
    }
}

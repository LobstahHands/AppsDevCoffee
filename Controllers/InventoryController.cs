using AppsDevCoffee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        // POST method to update or add an origin type
        [HttpPost]
        public IActionResult UpdateInventory(int id, OriginType originType)
        {
            if (id > 0) // Update existing record
            {
                var existingOriginType = Context.OriginTypes.Find(id);
                if (existingOriginType == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    existingOriginType.Country = originType.Country;
                    existingOriginType.SupplierNotes = originType.SupplierNotes;
                    existingOriginType.RoasterNotes = originType.RoasterNotes;
                    existingOriginType.CostPerOz = originType.CostPerOz;

                    Context.Update(existingOriginType);
                    Context.SaveChanges();
                    return RedirectToAction(nameof(InventoryList));
                }

                return View(existingOriginType);
            }
            else // Add new record
            {
                if (ModelState.IsValid)
                {
                    Context.Add(originType);
                    Context.SaveChanges();
                    return RedirectToAction(nameof(InventoryList));
                }

                return View(originType);
            }
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

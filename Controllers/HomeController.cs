using AppsDevCoffee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppsDevCoffee.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}

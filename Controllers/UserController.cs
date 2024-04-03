using Microsoft.AspNetCore.Mvc;
using AppsDevCoffee.Models;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace AppsDevCoffee.Controllers
{
    public class UserController(CoffeeAppContext ctx) : Controller
    {
        private CoffeeAppContext Context { get; set; } = ctx;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            //add Encryption for password



            var user = Context.Users.SingleOrDefault(u => u.Username.ToLower() == model.Username.ToLower() && u.Password == model.Password);

            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Employees");
            }

            ModelState.AddModelError("", "Invalid Username or Password");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


    }


    }

}

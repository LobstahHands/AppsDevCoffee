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

            // Hash the password entered by the user
            string hashedPassword = HashPassword(model.Password);


            var user = Context.Users.SingleOrDefault(u => u.Username.ToLower() == model.Username.ToLower() && u.Hashed == hashedPassword);

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
        //Logout
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        //Register

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                // Check if the username already exists
                var existingUser = Context.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View(model);
                }

                // Hash the password 
                string hashedPassword = HashPassword(model.Password);

                // Create a new user
                var newUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserTypeId = model.UserTypeId, // Assuming UserTypeId is part of UserModel
                    Username = model.Username,
                    Password = model.Password, // Storing plain password temporarily
                    Hashed = hashedPassword, // Store the hashed password
                    Active = 1, // Assuming newly registered users are active
                    DateAdded = DateTime.Now
                };

                // Add the user to the database
                Context.Users.Add(newUser);
                Context.SaveChanges();

                // Redirect to login page after successful registration
                return RedirectToAction("Login");
            }

            // If ModelState is not valid, return the view with validation errors
            return View(model);
        }

        //Method for password encryption
        private string HashPassword(string password)
        {
            // Implement your password hashing logic here
            return password;
        }




    }


    

}

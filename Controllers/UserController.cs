using Microsoft.AspNetCore.Mvc;
using AppsDevCoffee.Models;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AppsDevCoffee.Models.AppsDevCoffee.Models;
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
        public IActionResult Login(Login model)
        {

            // Hash the password entered by the user -- uncomment for production
            //string hashedPassword = PasswordHasher.HashPassword(model.Password);
            string hashedPassword = model.Password;



            var user = Context.Users.SingleOrDefault(u => u.Username.ToLower() == model.Username.ToLower() && u.Hashed == hashedPassword);

            if (user != null && user.UserStatus=="Active"/*&& PasswordHasher.VerifyPassword(user.Hashed,hashedPassword)*/)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                string action = "Index";
                string controller = "Admin";

                switch (user.UserTypeId)
                {
                    case 1:
                        action = "Index";
                        controller = "Admin";
                        break;
                    case 2:
                        action = "Index";
                        controller= "Home";
                        break;
                    case 3:
                        action = "Index";
                        controller = "Home";
                        break;
                    default:
                        action = "Index";
                        controller = "Home";
                        break;
                }

                 action = "Index";
                 controller = "Admin";
                
                return RedirectToAction(action,controller);
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

        //Register get
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        //Register post

        [HttpPost]
        public IActionResult Register(Register model)
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

                // Hash the password - uncomment for production
                //string hashedPassword = PasswordHasher.HashPassword(model.Password);
                
                //no encryption for testing.
                string hashedPassword = model.Password;

                // Create a new user
                var newUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserTypeId = 3, //Hardcoded to add as a general user. May leave the is for production and have admin switch to employee.
                    Username = model.Username,
                    Hashed = hashedPassword, // Store the hashed password
                    UserStatus = "Active", // switch default to Pending for production until approved by Admin 
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

    } 

}

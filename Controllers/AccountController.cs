using Microsoft.AspNetCore.Mvc;
using AppsDevCoffee.Models;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AppsDevCoffee.Models.AppsDevCoffee.Models;
using Azure.Identity;
namespace AppsDevCoffee.Controllers
{
    public class AccountController(CoffeeAppContext ctx) : Controller
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
        public IActionResult Login(VMLogin model)
        {

            // Hash the password entered by the user -- uncomment for production
            //string providedPassword = PasswordHasher.HashPassword(model.Password);
            
            string providedPassword = model.Password;

            AccountLog accountLog = new()
            {
                Username = model.Username,
                Email = null,
                CreatedDate = DateTime.Now
            };

            var user = Context.Users.SingleOrDefault(u => u.Username.ToLower() == model.Username);
            if (user != null)
            {
                bool validPassword = PasswordHasher.VerifyPassword(user.Hashed, providedPassword);

                if (user != null && user.UserStatus == "Active" && validPassword)
                {
                    var claims = new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // User ID
                    new Claim(ClaimTypes.Name, user.Username), //Username
                    new Claim("UserTypeId",user.UserTypeId.ToString()) //User Type Id
                };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    accountLog.Email = user.Email;
                    accountLog.LogResult = "Login Success";
                    Context.AccountLogs.Add(accountLog);
                    Context.SaveChanges();

                    string action = "Index";
                    string controller = "Home";

                    switch (user.UserTypeId)
                    {
                        case 1:
                            action = "Index";
                            controller = "Admin";
                            break;
                        case 2:
                            action = "Index";
                            controller = "Home";
                            break;
                        default:
                            action = "Index";
                            controller = "Home";
                            break;
                    }

                    return RedirectToAction(action, controller);
                }
                else if (user != null && user.UserStatus != "Active" && validPassword)
                {
                    //Valid login, but account status is still pending
                    accountLog.LogResult = "Blocked Login - Pending User";
                    Context.AccountLogs.Add(accountLog);
                    Context.SaveChanges();

                    ModelState.AddModelError("", "Account Pending, Unable to Login.");
                    return View(model);
                }
                else
                {
                    //unsuccesful login
                    //add to log
                    accountLog.LogResult = "Login Failure";
                    Context.AccountLogs.Add(accountLog);
                    Context.SaveChanges();
                    //model error
                    ModelState.AddModelError("", "Invalid Username or Password");
                    return View(model);
                }

            }
            else {
                ModelState.AddModelError("", "No account associated with that username.");
                return View();
            }
            
        }
        //Logout  - Absolute savage. instant logout. no confirmation. 
        public IActionResult Logout()
        {
            User user = new();

            int userId;
            if (int.TryParse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out userId))
            {
                user = Context.Users.FirstOrDefault(x => x.Id == userId);
            }
            
            //Create Log object
            AccountLog accountLog = new()
            {
                Username = user.Username,
                Email = user.Email,
                LogResult = "Logout",
                CreatedDate = DateTime.Now
            };

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
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
        public IActionResult Register(VMRegister model)
        {
            //Create Log object
            AccountLog accountLog = new()
            {
                Username = model.Username,
                Email = model.Email,    
                CreatedDate = DateTime.Now
            };


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
                string hashedPassword = PasswordHasher.HashPassword(model.Password);
                
                //no encryption for testing.
                //string providedPassword = model.Password;

                // Create a new user
                var newUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserTypeId = 3, //Hardcoded to add as a general user. May leave the is for production and have admin switch to employee.
                    Username = model.Username,
                    Hashed = hashedPassword, // Store the hashed password
                    UserStatus = "Pending", // switch default to Pending for production until approved by Admin 
                    DateAdded = DateTime.Now
                };

                // Add the user to the database
                Context.Users.Add(newUser);

                //update log result and add to the databse
                accountLog.LogResult = "New Registration";
                Context.AccountLogs.Add(accountLog);
          
                //Save Changes

                Context.SaveChanges();
                
                // Redirect to login page after successful registration
                return RedirectToAction("Login");
            }

            //Update Result and add to the db. 
            accountLog.LogResult = "New Registration";
            Context.AccountLogs.Add(accountLog);

            //Save Changes
            Context.SaveChanges();
            // If ModelState is not valid, return the view with validation errors
            return View(model);
        }



        // The GET method brings them to the edit view to edit their user details
        [HttpGet]
        public IActionResult Edit()
        {
            var userId = int.Parse(User.Identity.Name);
            var user = Context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // The POST method updates the database
        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                Context.Users.Update(user);
                Context.SaveChanges();
                return RedirectToAction("Index", "Home"); // Redirect to a suitable action after successful edit
            }
            return View(user);
        }

    } 

}

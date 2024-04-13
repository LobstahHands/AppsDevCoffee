using Microsoft.AspNetCore.Mvc;
using AppsDevCoffee.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Authorize(Policy = "AdminOnly")]
public class AdminController : Controller
{
    private readonly CoffeeAppContext Context;

    public AdminController(CoffeeAppContext context)
    {
        Context = context;
    }

    public IActionResult index()
    {
        return View();
    }

    public IActionResult UserList()
    {
        var users = Context.Users.Include(u => u.UserType).ToList();
        return View(users);
    }

    [Route("admin/userdetail/{id}")]
    public IActionResult UserDetail(int id)
    {
        User user = Context.Users
            .Include(u => u.UserType)   
            .FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpGet]
    public IActionResult EditUser(int id)
    {
        User user = Context.Users.Include(u => u.UserType).FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        ViewBag.UserTypes = Context.UserTypes.ToList();
        ViewBag.EditAction = "Edit a User";
        return View(user);
    }

    [HttpPost]
    public IActionResult EditUser(VMEditUser editUser)
    {
        ModelState.Remove("UserType");
        if (ModelState.IsValid)
        {
            var user = Context.Users
            .Where(u => u.Id == editUser.Id)
            .Include(u => u.UserType)
            .FirstOrDefault();


            user.FirstName = editUser.FirstName;
            user.LastName = editUser.LastName;  
            user.Username = editUser.Username;  
            user.Email = editUser.Email;
            user.UserTypeId = editUser.UserTypeId;
            user.UserStatus = editUser.UserStatus;


            Context.Users.Update(user);
            Context.SaveChanges();
            return RedirectToAction("UserDetail", new { id = user.Id });
        }
        ViewBag.UserTypes = Context.UserTypes.ToList(); 
        return View(editUser);
    }


    [HttpGet]
    public IActionResult AddUser()
    {
        ViewBag.UserTypes = Context.UserTypes.ToList();
        ViewBag.EditAction = "Add a User";
        return View("EditUser", new VMEditUser());

    }


    [HttpPost]
    public IActionResult AddUser(VMEditUser editUser)
    {
        if (ModelState.IsValid)
        {
            // Map the properties from VMEditUser to User model
            var user = new User
            {
                FirstName = editUser.FirstName,
                LastName = editUser.LastName,
                Email = editUser.Email,
                UserTypeId = editUser.UserTypeId,
                Username = editUser.Username,
                UserStatus = editUser.UserStatus

                //finish adding necessary properties
            };

            Context.Users.Add(user);
            Context.SaveChanges();

            return RedirectToAction("UserDetail", new { id = user.Id });
        }

        ViewBag.UserTypes = Context.UserTypes.ToList();
        return View("EditUser", editUser);
    }


    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        User user = Context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        Context.Users.Remove(user);
        Context.SaveChanges();
        return RedirectToAction("UserList");
    }


    //View Logs
    public IActionResult AccountLogs()
    {
        // Retrieve account logs from the database
        var logs = Context.AccountLogs.ToList();

        // Pass the logs to the view
        return View(logs);
    }
}

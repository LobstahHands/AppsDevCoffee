using Microsoft.AspNetCore.Mvc;
using AppsDevCoffee.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

//[Authorize]
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
        return View(user);
    }

    [HttpPost]
    public IActionResult EditUser(User user)
    {
        if (ModelState.IsValid)
        {
            Context.Users.Update(user);
            Context.SaveChanges();
            return RedirectToAction("UserDetail", new { id = user.Id });
        }
        ViewBag.UserTypes = Context.UserTypes.ToList(); 
        return View(user);
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
}

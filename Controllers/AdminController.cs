using Microsoft.AspNetCore.Mvc;
using AppsDevCoffee.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.Elfie.Extensions;

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
        // Fetch pending users
        var pendingUsers = Context.Users.Where(u => u.UserStatus == "Pending").ToList();

        // Fetch pending orders
        var pendingOrders = Context.Orders.Where(o => o.OrderStatus == "Pending").ToList();

        // Pass the data to the view
        var viewModel = new AdminIndexViewModel
        {
            PendingUsers = pendingUsers,
            PendingOrders = pendingOrders
        };

        return View(viewModel);
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

    [HttpGet]
    public IActionResult PendingOrderList()
    {
        // Fetch pending orders data from your database or wherever it's stored
        var pendingOrders = Context.Orders.Where(o => o.OrderStatus == "Pending").ToList();


        // Pass the pending orders data to the view
        return View(pendingOrders);
    }
    [HttpPost]
    public IActionResult PendingOrderList(int orderId)
    {
        // Retrieve the order from your context
        var order = Context.Orders.FirstOrDefault(o => o.Id == orderId);

        // Check if the order exists
        if (order == null)
        {
            // Handle the case where the order doesn't exist
            return NotFound(); 
        }

        // Update the order status to "filled"
        order.OrderStatus = "Filled"; 

        // Save changes back to the context
        Context.SaveChanges();

        // Redirect back to the PendingOrders view
        return RedirectToAction("Index");
    }


    // Action method for rendering the PendingUsers view
    public IActionResult PendingUserList()
    {
        // Fetch pending users data from your database or wherever it's stored
        var pendingUsers = ""; // Retrieve pending users here

            // Pass the pending users data to the view
            return View(pendingUsers);
    }
    [HttpPost]
    public IActionResult PendingUserList(int userId)
    {
        // Retrieve the user from your context
        var user = Context.Users.FirstOrDefault(u => u.Id == userId);

        // Check if the user exists
        if (user == null)
        {
            // Handle the case where the User doesn't exist
            return NotFound();
        }

        // Update the order status to "Active"
        user.UserStatus = "Active";

        // Save changes back to the context
        Context.SaveChanges();

        // Redirect back to index
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DeletePendingUser(int userId)
    {
        // Retrieve the user from your context
        var user = Context.Users.FirstOrDefault(u => u.Id == userId);

        // Check if the user exists
        if (user == null)
        {
            // Handle the case where the User doesn't exist
            return NotFound();
        }

        // Remove the user from the context
        Context.Users.Remove(user);

        // Save changes back to the context
        Context.SaveChanges();

        // Redirect back to index
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Analytics()
    {

        //All analytics are since the oldest order

        // Calculate the number of weeks since January 1st
        DateTime startOfAnalytics = Context.Orders.Min(o => o.OrderDate); 
        int weeksSinceStartOfAnalytics = (int)Math.Ceiling((DateTime.Now - startOfAnalytics).TotalDays / 7);
        // Calculate the count of orders
        var countOfOrders = Context.Orders.Where(o=>o.OrderDate >= startOfAnalytics).Count();
        var countOfOrderItems = Context.OrderItems.Where(o => o.Order.OrderDate >= startOfAnalytics).Count();
        float sumOfTotalCost = (float)Context.Orders.Sum(o => o.TotalCost);
        float sumOfTotalPaid = (float)Context.Orders.Sum(o => o.TotalPaid);

        var avgOrdersPerWeek = countOfOrders / weeksSinceStartOfAnalytics;
        var avgOrderItemsPerWeek = countOfOrders / weeksSinceStartOfAnalytics;
        var avgItemsPerOrder = countOfOrderItems / countOfOrders;
        var avgCostPerOrder = sumOfTotalCost / countOfOrders;
        var unpaidOrderTotal = sumOfTotalCost - sumOfTotalPaid;

            // Pass the results to the view
        var model = new AnalyticsViewModel
        {
            StartOfAnalytics = startOfAnalytics,
            WeeksSinceStartOfYear = weeksSinceStartOfAnalytics,
            CountOfOrders = countOfOrders,
            CountOfOrderItems = countOfOrderItems,
            SumOfTotalCost = sumOfTotalCost,
            SumOfTotalPaid = sumOfTotalPaid,
            AvgOrdersPerWeek = avgOrdersPerWeek,
            AvgOrderItemsPerWeek = avgOrderItemsPerWeek,
            AvgItemsPerOrder = avgItemsPerOrder,
            AvgCostPerOrder = avgCostPerOrder,
            UnpaidOrderTotal = unpaidOrderTotal
        };

        return View(model);
    } 
    
}





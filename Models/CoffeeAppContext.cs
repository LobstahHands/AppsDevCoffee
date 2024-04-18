using Microsoft.EntityFrameworkCore;

namespace AppsDevCoffee.Models
{
    public class CoffeeAppContext : DbContext
    {
        public CoffeeAppContext(DbContextOptions<CoffeeAppContext> options) : base(options)
        {
        }

        // Define your DbSet properties for each of your database tables
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<OriginType> OriginTypes { get; set; }
        public DbSet<RoastType> RoastTypes { get; set; }
        public DbSet<AccountLog> AccountLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserType>().HasData(
                new UserType { Id = 1, Description = "Admin" },
                new UserType { Id = 2, Description = "Employee" },
                new UserType { Id = 3, Description = "User" }
            );


            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@example.com",
                    UserTypeId = 1,
                    Username = "admin",
                    Hashed = "PassPass1!", // Replace with hashed password
                    UserStatus = "Active",
                    DateAdded = DateTime.Now
                },
                new User
                {
                    Id = 2,
                    FirstName = "JohnTest",
                    LastName = "Doe",
                    Email = "john@example.com",
                    UserTypeId = 2,
                    Username = "john",
                    Hashed = "PassPass1!", // Replace with hashed password
                    UserStatus = "Active",
                    DateAdded = DateTime.Now
                },
                new User
                {
                    Id = 3,
                    FirstName = "JaneTest",
                    LastName = "Doe",
                    Email = "jane@example.com",
                    UserTypeId = 3,
                    Username = "jane",
                    Hashed = "PassPass1!", // Replace with hashed password
                    UserStatus = "Pending",
                    DateAdded = DateTime.Now
                }
            );


            modelBuilder.Entity<OriginType>().HasData(
                new OriginType {
                    OriginTypeId = 1,
                    Country = "Colombia",
                    SupplierNotes = "Versatile",
                    RoasterNotes = "Bold at medium, rich at dark",
                    CostPerOz = 1.0f,
                    Active = 1
                },
                new OriginType
                {
                    OriginTypeId = 2,
                    Country = "Costa Rica",
                    SupplierNotes = "Bright and Punch",
                    RoasterNotes = "Fruity and acidic at light, rounds out to a bold at medium",
                    CostPerOz = 1.0f,
                    Active = 1
                },
                new OriginType
                {
                    OriginTypeId = 3,
                    Country = "Mexico",
                    SupplierNotes = "Decaf - floral and honey tasting notes",
                    RoasterNotes = "Bold at medium, rich at dark",
                    CostPerOz = 1.0f,
                    Active = 1
                }
            );

            modelBuilder.Entity<RoastType>().HasData(

                new RoastType
                {
                    RoastTypeId = 1,
                    Description = "Light",
                    Active = 1
                },
                new RoastType
                {
                    RoastTypeId = 2,
                    Description = "Medium",
                    Active = 1
                },
                new RoastType
                {
                    RoastTypeId = 3,
                    Description = "Dark",
                    Active = 1    
                }

            );
        

            // Seed OrderItems
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    OrderItemId = 1,
                    OrderId = 1, // Order ID
                    OriginTypeId = 1,
                    RoastTypeId = 1,
                    OzQuantity = 15.00f,
                    Subtotal = 15.00f

                }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    UserId = 1, // Admin user
                    TotalPaid = 15.0f,
                    OrderDate = DateTime.Now,
                    OrderStatus = "Filled",
                    SubtotalCost = 15.0f,
                    PriceAdjustment = 0.0f,
                    TotalCost = 15.0f
                },
                new Order
                {
                    Id = 2,
                    UserId = 1, // Admin user
                    TotalPaid = 0.0f,
                    OrderDate = DateTime.Now,
                    OrderStatus = "Pending",
                    SubtotalCost = 25.0f,
                    PriceAdjustment = 0.0f,
                    TotalCost = 25.0f
                }
            ); 

            // Configure relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserType)
                .WithMany()
                .HasForeignKey(u => u.UserTypeId);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.OriginType)
                .WithMany()
                .HasForeignKey(oi => oi.OriginTypeId);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.RoastType)
                .WithMany()
                .HasForeignKey(oi => oi.RoastTypeId);
        }
    }
}

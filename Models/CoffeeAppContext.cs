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
        public DbSet<CurrentInventory> CurrentInventories { get; set; }
        public DbSet<Roast> Roasts { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<OriginType> OriginTypes { get; set; }
        public DbSet<RoastType> RoastTypes { get; set; }
        
        public DbSet<InventoryLog> InventoryLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                .HasOne(oi => oi.CurrentInventory)
                .WithMany()
                .HasForeignKey(oi => oi.CurrentInventoryId);

            modelBuilder.Entity<CurrentInventory>()
                .HasOne(ci => ci.Roast)
                .WithMany()
                .HasForeignKey(ci => ci.RoastId);

            modelBuilder.Entity<CurrentInventory>()
                .HasOne(ci => ci.OriginType)
                .WithMany()
                .HasForeignKey(ci => ci.OriginTypeId);
        }
    }
}

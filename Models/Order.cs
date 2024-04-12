namespace AppsDevCoffee.Models
{
    public class Order
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public float? TotalPaid { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public float? SubtotalCost { get; set; }
        public float? PriceAdjustment { get; set; }
        public float? TotalCost { get; set; }

        // Navigation property
        public User User { get; set; }

        // Navigation property
        public List<OrderItem> OrderItems { get; set; }
    }
}

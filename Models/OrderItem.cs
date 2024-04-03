namespace AppsDevCoffee.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CurrentInventoryId { get; set; }
        public float OzQuantity { get; set; }
        public float CostPerOz { get; set; }

        // Navigation property
        public Order Order { get; set; }

        // Navigation property
        public CurrentInventory CurrentInventory { get; set; }
    }
}

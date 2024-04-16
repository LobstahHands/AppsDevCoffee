namespace AppsDevCoffee.Models
{
    public class CreateItemViewModel { 
    
        public int? OriginTypeId { get; set; }
        public int RoastTypeId { get; set; }
        public float OzQuantity { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}

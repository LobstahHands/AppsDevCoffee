namespace AppsDevCoffee.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int OriginTypeId { get; set; }
        public int RoastTypeId { get; set; }
        public float OzQuantity { get; set; }
        public float Subtotal { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public OriginType OriginType { get; set; }
        public RoastType RoastType { get; set; }

        public void CalculateSubtotal()
        {
            // Check if OriginType is loaded
            if (OriginType != null)
            {
                Subtotal = OzQuantity * OriginType.CostPerOz;
            }
        }


        //code to make a new order item and calc the subtotal 
        //OrderItem orderItem = new OrderItem
        //{
        //    // set other properties
        //};

        //// Calculate subtotal
        //orderItem.CalculateSubtotal();

        //// Add the OrderItem to the context and save changes
        //context.OrderItems.Add(orderItem);
        //context.SaveChanges();





    }
}

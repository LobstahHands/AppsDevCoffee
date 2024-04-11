namespace AppsDevCoffee.Models
{
    public class OriginType
    {
        public int OriginTypeId { get; set; }
        public string Country { get; set; }
        public string SupplierNotes { get; set; }
        public string RoasterNotes { get; set; }
        public float CostPerOz { get; set; }
        public int Active { get; set; }
    }
}

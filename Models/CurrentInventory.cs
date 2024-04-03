using AppsDevCoffee.Models.TypeTables;

namespace AppsDevCoffee.Models
{
    public class CurrentInventory
    {
        public int Id { get; set; }
        public int OzQuantity { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateLastModified { get; set; }
        public int RoastId { get; set; }
        public float PerOzPrice { get; set; }
        public int OriginTypeId { get; set; }
        public int TierTypeId { get; set; }

        // Navigation property
        public Roast Roast { get; set; }

        // Navigation property
        public OriginType OriginType { get; set; }

        // add property for tier?

    }
}

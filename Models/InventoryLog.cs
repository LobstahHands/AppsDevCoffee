using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppsDevCoffee.Models
{
    public class InventoryLog
    {
        [Key]
        public int Id { get; set; }
        public int OzQuantity { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateLastModified { get; set; }
        public int RoastId { get; set; }
        public float PerOzPrice { get; set; }
        public int OriginTypeId { get; set; }
        public int TierTypeId { get; set; }
        // Additional fields for tracking changes

        [Required]
        public DateTime ChangeDate { get; set; }

        public int UserId { get; set; } // Assuming a user ID is used for tracking who made the change

        // Navigation property
        [ForeignKey("RoastId")]
        public Roast Roast { get; set; }

        // Navigation property
        [ForeignKey("OriginTypeId")]
        public OriginType OriginType { get; set; }
    }
}

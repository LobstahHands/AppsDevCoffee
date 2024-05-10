using System.ComponentModel.DataAnnotations;

namespace AppsDevCoffee.Models
{
    public class OriginType
    {
        public int OriginTypeId { get; set; }

        [Required(ErrorMessage = "Country is a required field")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Country must not contain numerical values")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Supplier Notes is a required field")]
        public string? SupplierNotes { get; set; }
        [Required(ErrorMessage = "Roatser Notes is a required field")]
        public string? RoasterNotes { get; set; }

        [Required(ErrorMessage = "Cost is a required field")]
        [Range(0.01, 500.00, ErrorMessage = "Cost must be between $0.01 and $500.00")]
        public float CostPerOz { get; set; }
        public int Active { get; set; }
    }
}

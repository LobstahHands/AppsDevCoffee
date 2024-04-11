using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppsDevCoffee.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
}

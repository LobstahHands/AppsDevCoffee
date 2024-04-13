using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppsDevCoffee.Models
{
    public class AccountLog
    {
        [Key]
        public int Id { get; set; }
        public string LogResult { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }   
        public DateTime CreatedDate { get; set; }
    }
}

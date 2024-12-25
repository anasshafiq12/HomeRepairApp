using System.ComponentModel.DataAnnotations;

namespace HouseRepairApp.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; } // Primary key

        public MyUser User { get; set; } // Associated user (optional)

        // Navigation property to the associated Cart

        public Cart Cart { get; set; }
    }
}

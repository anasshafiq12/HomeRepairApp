using System.ComponentModel.DataAnnotations;

namespace HouseRepairApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public MyUser User { get; set; }
        public List<CartItem> cartItems { get; set; }
    }
}

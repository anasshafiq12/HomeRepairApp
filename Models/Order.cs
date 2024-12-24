using System.ComponentModel.DataAnnotations;

namespace HouseRepairApp.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public MyUser User { get; set; }
        public List<CartItem> cartItems { get; set; }
        public float TotalPrice { get; set; } = 0;
        public void SetTotalPrice()
        {
            TotalPrice = 0;
            foreach (var item in cartItems)
            {
                TotalPrice += item.Price * item.SelectedQuantityByUser;
            }
        }
    }
}

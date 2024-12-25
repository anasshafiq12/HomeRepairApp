using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseRepairApp.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; } // Primary key

        public string UserId { get; set; }
        // List of cart items
        public ICollection<CartItem> CartItems { get; set; }

        public float TotalPrice { get; set; } = 0;


        // Logic to update item quantities and total price
        public void SetItemPriceAndQuantity(int id, int quantity)
        {
            foreach (var item in CartItems)
            {
                if (item.ItemId == id)
                {
                    item.PriceWrtQuanity = item.Price * quantity;
                    item.SelectedQuantityByUser = quantity;
                }
            }
        }

        public void SetTotalPrice()
        {
            TotalPrice = 0;
            foreach (var item in CartItems)
            {
                if (item.SelectedQuantityByUser == 0)
                    item.SelectedQuantityByUser = 1;

                if (item.PriceWrtQuanity == 0)
                    item.PriceWrtQuanity = item.Price;

                TotalPrice += item.PriceWrtQuanity;
            }
        }
    }
}

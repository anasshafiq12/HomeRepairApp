namespace HouseRepairApp.Models
{
    public class Cart
    {
        public List<CartItem> CartItems { get; set; }
        public float TotalPrice { get; set; } = 0;
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
        public void RemoveItem(int id)
        {
            foreach (var item in CartItems)
            {
                if (item.ItemId == id)
                {
                    CartItems.Remove(item);
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
namespace HouseRepairApp.Models
{
    public class Cart
    {
        public List<CartItem> CartItems { get; set; }
        public float TotalPrice { get; set; } = 0;
        public void SetItemPriceAndQuantity(int id,int quantity)
        {
            foreach (var item in CartItems)
            {
                if (item.ItemId == id)
                {
                    item.Price = item.Price * quantity;
                    item.SelectedQuantityByUser = quantity;
                }
            }
        }
        public void SetTotalPrice()
        {
            TotalPrice = 0;
            foreach (var item in CartItems)
            {
                TotalPrice += item.Price;
            }
        }
    }
}

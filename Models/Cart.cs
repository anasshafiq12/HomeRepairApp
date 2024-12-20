namespace HouseRepairApp.Models
{
    public class Cart
    {
        public List<CartItem> CartItems { get; set; }
        public float TotalPrice { get; set; }
        public void SetItemPrice (int id,int quantity)
        {
            foreach (var item in CartItems)
            {
                if(item.ItemId == id)
                    item.Price = item.Price * quantity;
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

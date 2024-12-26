namespace HouseRepairApp.Models
{
    public class Order
    {
        public MyUser User { get; set; }
        public Cart Cart { get; set; }
        public List<SelectedCartItem> cartItems {  get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace HouseRepairApp.Models
{
    public class CartItem
    {
        
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
    }
}

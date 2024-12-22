using System.ComponentModel.DataAnnotations;

namespace HouseRepairApp.Models
{
    public class CartItem
    {
        [Key]
        public int ItemId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int AvailableQuantity { get; set; }
        public int SelectedQuantity { get; set; } = 1;
    }
}

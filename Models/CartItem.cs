using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HouseRepairApp.Models
{
    public class CartItem
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public int AvailableQuantity { get; set; }
        public float PriceWrtQuanity { get; set; }
        [DefaultValue(1)]
        public int SelectedQuantityByUser { get; set; }
    }
}

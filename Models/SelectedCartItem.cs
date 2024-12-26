using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HouseRepairApp.Models
{
    public class SelectedCartItem 
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public float Price { get; set; }
        public string ImageUrl { get; set; }
        public int AvailableQuantity { get; set; }
        public float PriceWrtQuanity { get; set; }
        [DefaultValue(1)]
        public int SelectedQuantityByUser { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}

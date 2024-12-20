using System.ComponentModel.DataAnnotations;

namespace HouseRepairApp.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
    }
}

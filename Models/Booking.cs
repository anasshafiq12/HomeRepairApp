using System.ComponentModel.DataAnnotations;

namespace HouseRepairApp.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public string Name {  get; set; }
        public string Phone { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; } 
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}

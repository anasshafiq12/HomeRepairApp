namespace HouseRepairApp.Models
{
    public class Booking
    {
        public string Name {  get; set; }
        public string Phone { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; } 
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}

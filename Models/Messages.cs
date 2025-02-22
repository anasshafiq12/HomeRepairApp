using System.ComponentModel.DataAnnotations;

namespace HouseRepairApp.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public IEnumerable<string> MessagesText { get; set; }
    }
}

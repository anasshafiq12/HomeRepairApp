using Microsoft.AspNetCore.Identity;

namespace HouseRepairApp.Models
{
    public class MyUser:IdentityUser
    {
        public string Name {  get; set; }
        public string Phone {  get; set; }
        public string Address { get; set; }
    }
}

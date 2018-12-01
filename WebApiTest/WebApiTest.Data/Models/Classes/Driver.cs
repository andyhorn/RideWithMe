using System.Collections.Generic;
using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Data.Models.Classes
{
    public class Driver : IDriver
    {
        //public Driver() { }
        //public Driver(User user, Vehicle vehicle)
        //{
        //    User = user;
        //    Vehicle = vehicle;
        //}
        public Driver()
        {
            User = new User();
            Vehicle = new Vehicle();
            RideHistory = new List<IRide>();
        }
        public int Id { get; set; }
        public IUser User { get; set; }
        public IVehicle Vehicle { get; set; }
        public List<IRide> RideHistory { get; set; }
    }
}
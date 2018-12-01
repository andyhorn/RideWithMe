using System.Collections.Generic;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.Models.Classes
{
    public class Driver : IDriver
    {
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
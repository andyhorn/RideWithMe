using System;
using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Data.Models.Classes
{
    public class Ride : IRide
    {
        public IUser Rider { get; set; }
        public IUser Driver { get; set; }
        public IVehicle Vehicle { get; set; }
        public string Destination { get; set; }
        public string PickupLocation { get; set; }
        public double Distance { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
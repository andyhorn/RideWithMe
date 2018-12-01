using System;
using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Data.Models.Classes
{
    public class Ride : IRide
    {
        //public Ride() { }
        //public Ride(User rider, User driver, Vehicle vehicle)
        //{
        //    Rider = rider;
        //    Driver = driver;
        //    Vehicle = vehicle;
        //}
        public Ride()
        {
            Rider = new User();
            Driver = new User();
            Vehicle = new Vehicle();
        }
        public int Id { get; set; }
        public IUser Rider { get; set; }
        public IUser Driver { get; set; }
        public IVehicle Vehicle { get; set; }
        public string Destination { get; set; }
        public string PickupLocation { get; set; }
        public double Distance { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }
    }
}
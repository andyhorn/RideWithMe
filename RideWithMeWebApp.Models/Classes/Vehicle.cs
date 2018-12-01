using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.Models.Classes
{
    public class Vehicle : IVehicle
    {
        public Vehicle()
        {
            Driver = new User();
        }
        public int Id { get; set; }
        public IUser Driver { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string License { get; set; }
    }
}
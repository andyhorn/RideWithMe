using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Data.Models.Classes
{
    public class Vehicle : IVehicle
    {
        //public Vehicle() {}

        //public Vehicle(User driver)
        //{
        //    Driver = driver;
        //}

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
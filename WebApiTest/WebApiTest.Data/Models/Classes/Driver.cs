using System.Collections.Generic;
using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Data.Models.Classes
{
    public class Driver : IDriver
    {
        public int Id { get; set; }
        public IUser User { get; set; }
        public IVehicle Vehicle { get; set; }
        public List<IRide> RideHistory { get; set; }
    }
}
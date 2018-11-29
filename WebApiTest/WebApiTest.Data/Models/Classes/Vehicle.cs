﻿using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Data.Models.Classes
{
    class Vehicle : IVehicle
    {
        public int VehicleId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string License { get; set; }
    }
}
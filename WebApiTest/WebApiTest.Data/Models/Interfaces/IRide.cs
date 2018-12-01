using System;

namespace WebApiTest.Data.Models.Interfaces
{
    public interface IRide
    {
        int Id { get; set; }
        IUser Rider { get; set; }
        IUser Driver { get; set; }
        IVehicle Vehicle { get; set; }
        string Destination { get; set; }
        string PickupLocation { get; set; }
        double Distance { get; set; }
        DateTime RequestTime { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
    }
}
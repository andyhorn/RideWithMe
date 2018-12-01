using System.Collections.Generic;

namespace WebApiTest.Data.Models.Interfaces
{
    public interface IDriver
    {
        int Id { get; set; }
        IUser User { get; set; }
        IVehicle Vehicle { get; set; }
        List<IRide> RideHistory { get; set; }
    }
}
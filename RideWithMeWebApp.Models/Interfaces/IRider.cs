using System.Collections.Generic;

namespace RideWithMeWebApp.Models.Interfaces
{
    public interface IRider
    {
        IUser User { get; set; }
        int RiderId { get; set; }
        List<IRide> RideHistory { get; set; }
    }
}
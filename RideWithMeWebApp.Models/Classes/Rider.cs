using System.Collections.Generic;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.Models.Classes
{
    public class Rider : IRider
    {
        public Rider()
        {
            User = new User();
            RideHistory = new List<IRide>();
        }
        public IUser User { get; set; }
        public int RiderId { get; set; }
        public List<IRide> RideHistory { get; set; }
    }
}
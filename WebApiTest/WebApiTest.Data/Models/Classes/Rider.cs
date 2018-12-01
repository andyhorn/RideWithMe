using System.Collections.Generic;
using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Data.Models.Classes
{
    public class Rider : IRider
    {
        //public Rider() { }
        //public Rider(User user, List<IRide> rideHistory)
        //{
        //    User = user;
        //    RideHistory = rideHistory;
        //}
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
using System.Collections.Generic;
using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Data.Models.Classes
{
    public class Rider : IRider
    {
        public IUser User { get; set; }
        public int RiderId { get; set; }
        public List<IRide> RideHistory { get; set; }
    }
}
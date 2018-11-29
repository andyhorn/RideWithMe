using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApiTest.Data.Models.Classes;
using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RidesController : Controller
    {
        private readonly IDataProvider _dataProvider;

        public RidesController()
        {
            _dataProvider = new DataProvider();
        }

        [HttpGet("userId/{id}")]
        public List<IRide> GetByUserId(long id)
        {
            return _dataProvider.GetRidesByUserId(id);
        }

        [HttpGet("rideId/{id}")]
        public List<IRide> GetByRideId(long id)
        {
            return _dataProvider.GetRidesById(id);
        }

        [HttpGet("all")]
        public List<IRide> GetAllRides()
        {
            return _dataProvider.GetAllRides();
        }

        [HttpPost]
        public void Post([FromBody] IRide ride)
        {
            _dataProvider.AddNewRide(ride);
        }
    }
}
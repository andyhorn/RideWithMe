using System;
using System.Threading.Tasks;
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
            //Console.WriteLine("RidesController created.");
        }

        [HttpGet("userId/{id}")]
        public WebResponse GetByUserId(long id)
        {
            var response = new WebResponse();
            response.Data["rideHistory"] = _dataProvider.GetRidesByUserId(id);
            return response;
        }

        [HttpGet("rideId/{id}")]
        public WebResponse GetByRideId(long id)
        {
            var response = new WebResponse();
            response.Data["rideHistory"] = _dataProvider.GetRidesById(id);
            return response;
        }

        [HttpGet("all/")]
        public WebResponse GetAllRides()
        {
            var response = new WebResponse();
            response.Data["rideHistory"] = _dataProvider.GetAllRides();
            return response;
        }


        [HttpPost]
        [ProducesResponseType(200)]
        public OkResult Post([FromBody] Ride ride)
        {
            _dataProvider.AddNewRide(ride);

            return Ok();
        }

        [HttpGet("test")]
        public IRide Get()
        {
            var newRide = new Ride();
            newRide.Rider = new User
                {FirstName = "Andy", LastName = "Horn", Email = "andyjhorn@gmail.com", Id = 1, UserType = 0};
            newRide.Driver = new User
            {
                FirstName = "Krystal",
                LastName = "Cruz",
                Email = "kcruz191@gmail.com",
                Id = 2,
                UserType = 1
            };
            newRide.Vehicle = new Vehicle()
            {
                Color = "White",
                Driver = null,
                Id = 1,
                License = "771JJA",
                Make = "Hyundai",
                Model = "Elantra",
                Year = 2012
            };
            newRide.Destination = "Portland, OR";
            newRide.Distance = 15.0;
            newRide.RequestTime = new DateTime(2018, 11, 30);
            newRide.StartTime = new DateTime(2018, 11, 30);
            newRide.EndTime = new DateTime(2018, 11, 30);
            newRide.PickupLocation = "Wilsonville, OR";

            return newRide;
        }
    }
}
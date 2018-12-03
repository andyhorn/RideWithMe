using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RideWithMeWebApp.DataProvider.Models.Interfaces;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IDataProvider _dataProvider;

        public AdminController()
        {
            _dataProvider = new DataProvider.Models.Classes.DataProvider();
        }

        [HttpGet("users/all")]
        public IList<IUser> GetAllUsers()
        {
            var collection = _dataProvider.GetAllUsers();
            return collection;
        }

        [HttpGet("users/{id}")]
        public IUser GetUserById(long id)
        {
            return _dataProvider.GetUserById(id);
        }

        [HttpGet("vehicles/all")]
        [ProducesResponseType(200), ProducesResponseType(204), ProducesResponseType(500)]
        public IList<IVehicle> GetAllVehicles()
        {
            var collection = _dataProvider.GetAllVehicles();
            return collection;
        }

        [HttpGet("rides/all")]
        public IList<IRide> GetAllRides()
        {
            var collection = _dataProvider.GetAllRides();
            return collection;
        }
    }
}
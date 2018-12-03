using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideWithMeWebApp.DataProvider.Models.Interfaces;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IDataProvider _dataProvider;

        public VehiclesController()
        {
            _dataProvider = new DataProvider.Models.Classes.DataProvider();
        }

        [HttpGet("user/{id}")]
        public IList<IVehicle> GetVehiclesByUser(int id)
        {
            var collection = _dataProvider.GetVehiclesByUserId(id);
            return collection;
        }
    }
}
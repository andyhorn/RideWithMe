using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RideWithMeWebApp.DataProvider.Models.Interfaces;

namespace RideWithMeWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : Controller
    {
        private readonly IDataProvider _dataProvider;

        public UpdateController()
        {
            _dataProvider = new DataProvider.Models.Classes.DataProvider();
        }

        [HttpPut, HttpPost]
        [ProducesResponseType(200), ProducesResponseType(400)]
        public IActionResult Post([FromForm] Dictionary<string, string> content)
        {
            var targetTable = content["TargetTable"];
            var targetId = Convert.ToInt32(content["TargetId"]);
            var param = content["Param"];
            var newValue = content["NewValue"];
            var ok = false;
                switch (targetTable)
                {
                    case "Users":
                        ok = _dataProvider.UpdateUser(targetId, param, newValue);
                        break;
                    case "Vehicles":
                        ok = _dataProvider.UpdateVehicle(targetId, param, newValue);
                        break;
                    case "Rides":
                        ok = _dataProvider.UpdateRide(targetId, param, newValue);
                        break;
                    case "Logins":
                        ok = _dataProvider.UpdateLogin(targetId, param, newValue);
                        break;
                }

            if (ok)
                return Ok();
            return BadRequest();
        }
    }
}
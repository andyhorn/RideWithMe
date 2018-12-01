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
        public void Post(long targetId, string targetTable, string param, string newValue)
        {
                switch (targetTable)
                {
                    case "users":
                        _dataProvider.UpdateUser(targetId, param, newValue);
                        break;
                    case "vehicles":
                        _dataProvider.UpdateVehicle(targetId, param, newValue);
                        break;
                    case "rides":
                        _dataProvider.UpdateRide(targetId, param, newValue);
                        break;
                    case "logins":
                        _dataProvider.UpdateLogin(targetId, param, newValue);
                        break;
                }
        }
    }
}
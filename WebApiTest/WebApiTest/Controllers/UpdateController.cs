using Microsoft.AspNetCore.Mvc;
using WebApiTest.Data.Models.Classes;
using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : Controller
    {
        private readonly IDataProvider _dataProvider;

        public UpdateController()
        {
            _dataProvider = new DataProvider();
        }

        [HttpPut, HttpPost]
        public void Post(long targetId, string targetTable, string param, string newValue)
        {
            //var sqlCommandString = $"UPDATE{table} SET '{param}' = '{newValue}' WHERE User "
            switch (targetTable)
            {
                case "users":
                    UpdateUsers(targetId, param, newValue);
                    break;
                case "vehicles":
                    UpdateVehicles(targetId, param, newValue);
                    break;
                case "rides":
                    UpdateRides(targetId, param, newValue);
                    break;
                case "logins":
                    UpdateLogins(targetId, param, newValue);
                    break;
            }
        }

        private void UpdateUsers(long targetId, string param, string newValue)
        {

        }

        private void UpdateVehicles(long targetId, string param, string newValue)
        {

        }

        private void UpdateRides(long targetId, string param, string newValue)
        {

        }

        private void UpdateLogins(long targetId, string param, string newValue)
        {

        }
    }
}
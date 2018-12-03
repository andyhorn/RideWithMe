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
    }
}
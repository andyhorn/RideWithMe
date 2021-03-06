﻿using Microsoft.AspNetCore.Mvc;
using RideWithMeWebApp.Authentication.Classes;
using RideWithMeWebApp.Authentication.Interfaces;
using RideWithMeWebApp.DataProvider.Models.Interfaces;

namespace RideWithMeWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IAuthenticator _authenticator;

        public LoginController()
        {
            //IDataProvider dataProvider = new DataProvider.Models.Classes.DataProvider();
            _authenticator = new Authenticator(new DataProvider.Models.Classes.DataProvider());
        }

        // Get user based on login
        // TODO: Convert login to a POST request for higher security
        [HttpGet]
        public WebResponse Get(string email, string password)
        {
            var result = _authenticator.GetUser(email, password); // Will validate login and retrieve user data

            var response = new WebResponse {Message = result != null ? "Success!" : "Error!"};
            response.Data["user"] = result;

            return response;
        }
    }
}
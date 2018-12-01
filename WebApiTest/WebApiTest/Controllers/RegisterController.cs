﻿using Microsoft.AspNetCore.Mvc;
using WebApiTest.Authentication.Classes;
using WebApiTest.Authentication.Interfaces;
using WebApiTest.Data.Models.Classes;
using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        private readonly IAuthenticator _authenticator;

        public RegisterController()
        {
            IDataProvider dataProvider = new DataProvider();
            _authenticator = new Authenticator(dataProvider);
        }

        [HttpPost]
        public WebResponse Post(string firstName, string lastName, string email, string password)
        {
            var newUser = new User {FirstName = firstName, LastName = lastName, Email = email};
            var response = new WebResponse();
            response.Data["user"] = newUser;

            if (!_authenticator.UserExists(email))
            {
                var success = _authenticator.RegisterNewUser(newUser, password);

                response.Message = success 
                    ? "User registered successfully." 
                    : "Error: Unable to register user.";
            }
            else
            {
                response.Message = "Error: User already exists.";
            }

            return response;
        }
    }
}
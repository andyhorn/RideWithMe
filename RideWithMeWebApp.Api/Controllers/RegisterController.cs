using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RideWithMeWebApp.Authentication.Classes;
using RideWithMeWebApp.Authentication.Interfaces;
using RideWithMeWebApp.DataProvider.Models.Interfaces;
using RideWithMeWebApp.Models.Classes;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        private readonly IAuthenticator _authenticator;

        public RegisterController()
        {
            IDataProvider dataProvider = new DataProvider.Models.Classes.DataProvider();
            _authenticator = new Authenticator(dataProvider);
        }

        [HttpPost]
        [ProducesResponseType(200), ProducesResponseType(400)]
        public ActionResult Post([FromForm] Dictionary<string, string> data)
        {
            var firstName = data["FirstName"];
            var lastName = data["LastName"];
            var email = data["Email"];
            var password = data["Password"];
            var userType = Convert.ToInt32(data["UserType"]);

            //var response = new WebResponse();
            if (!string.IsNullOrEmpty(firstName)
                && !string.IsNullOrEmpty(lastName)
                && !string.IsNullOrEmpty(email)
                && !string.IsNullOrEmpty(password)) // If all parameters are included, continue with registration:
            {
                if (!_authenticator.UserExists(email)) // Only register if the user doesn't already exist
                {
                    // RegisterNewUser will return a bool indicating whether or not it was successful
                    var success = _authenticator.RegisterNewUser(new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        UserType = userType
                    }, password);

                    if (success)
                    {
                        return Ok();
                        // TODO: Clean up RegisterController
                        // TODO: Make all controllers return easy to deserialize (and expected) objects.
                    }

                    return BadRequest();
                }

                return BadRequest();
            }

            return BadRequest();
        }

    }
}
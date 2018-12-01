using Microsoft.AspNetCore.Mvc;
using RideWithMeWebApp.Authentication.Classes;
using RideWithMeWebApp.Authentication.Interfaces;
using RideWithMeWebApp.DataProvider.Models.Interfaces;
using RideWithMeWebApp.Models.Classes;

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
        public WebResponse Post(string firstName, string lastName, string email, string password)
        {
            var response = new WebResponse();
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
                        Email = email
                    }, password);

                    if (success)
                    {
                        var newUser = _authenticator.GetUser(email, password);
                        response.Message = "User registered successfully.";
                        response.Data["user"] = newUser;
                    }
                    else
                    {
                        response.Message = "Error: Unable to register user.";
                    }

                    //response.Message = success
                    //    ? "User registered successfully."
                    //    : "Error: Unable to register user.";
                    //response.Data["user"] = newUser;
                }
                else // Indicate the user already exists
                {
                    response.Message = "Error: User already exists.";
                }
            }
            else // Cannot register without all required parameters
            {
                response.Message = "Invalid submission. All parameters required.";
            }
            
            return response;
        }
    }
}
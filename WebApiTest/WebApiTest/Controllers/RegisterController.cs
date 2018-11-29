using Microsoft.AspNetCore.Mvc;
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
            var response = new WebResponse();
            if (!_authenticator.UserExists(email))
            {
                var success = _authenticator.RegisterNewUser(
                    email
                    , password
                    , firstName
                    , lastName);

                if (success)
                    response.Message = "User registered successfully.";
                else
                {
                    response.Message = "Error: Unable to register user.";
                    response.Data["email"] = email;
                    response.Data["firstName"] = firstName;
                    response.Data["lastName"] = lastName;
                    response.Data["password"] = password;
                }
            }
            else
            {
                response.Message = "Error: User already exists.";
            }

            return response;
        }
    }
}
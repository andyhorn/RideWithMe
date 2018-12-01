using Microsoft.AspNetCore.Mvc;
using WebApiTest.Authentication.Classes;
using WebApiTest.Authentication.Interfaces;
using WebApiTest.Data.Models.Classes;
using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IAuthenticator _authenticator;

        public LoginController()
        {
            IDataProvider dataProvider = new DataProvider();
            _authenticator = new Authenticator(dataProvider);
        }

        // Get user based on login
        [HttpGet]
        public WebResponse Get(string email, string password)
        {
            var result = _authenticator.ValidateUser(email, password);

            var response = new WebResponse {Message = result != null ? "Success!" : "Error!"};
            response.Data["user"] = result;

            return response;
        }
    }
}
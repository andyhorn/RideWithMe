using Microsoft.AspNetCore.Mvc;
using WebApiTest.Authentication.Classes;
using WebApiTest.Authentication.Interfaces;
using WebApiTest.Data.Classes;
using WebApiTest.Data.Interfaces;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    //[Produces("application/json")]
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
        public IUser Get(string email, string password)
        {
            //return _authenticator.ValidateUser(email, password);
            var result = _authenticator.ValidateUser(email, password);
            return result;
        }
    }
}
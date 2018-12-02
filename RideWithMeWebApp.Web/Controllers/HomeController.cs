using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using RideWithMeWebApp.Models.Classes;
using RideWithMeWebApp.Models.Interfaces;
using WebRequest = System.Net.WebRequest;

namespace RideWithMeWebApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUser _currentUser;
        private static readonly HttpClient client = new HttpClient();

        private const string URL = "https://ridewithmeapp.azurewebsites.net/api/";

        public HomeController()
        {
            _currentUser = null;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (_currentUser == null)
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            _currentUser = null;
            ViewBag.User = null;
            ViewBag.Vehicles = null;
            ViewBag.Rides = null;

            return RedirectToAction("Index");
        }


        public ActionResult Register()
        {
            return View();
        }
        public ActionResult SubmitLogin()
        {
            var email = Request.QueryString["Email"];
            var password = Request.QueryString["Password"];

            var result = GetResponse(URL + $"login?email={email}&password={password}");
            if (result != null && result.Message == "Success!")
            {
                var user = JsonConvert.DeserializeObject<User>(result.Data["user"].ToString());
                ViewBag.Status = 0;
                ViewBag.User = user;
                _currentUser = user;
                return View("Index");
            }

            ViewBag.Status = -1;
            return View("Login");
        }

        public async Task<ActionResult> RegisterUser()
        {
            var email = Request.QueryString["Email"];
            var password = Request.QueryString["Password"];
            var firstName = Request.QueryString["FirstName"];
            var lastName = Request.QueryString["LastName"];
            var userType = Request.QueryString["UserType"] == "Rider" ? 0 : 1;

            //var newUser = new User
            //{
            //    Email = email,
            //    FirstName = firstName,
            //    LastName = lastName,
            //    UserType = userType
            //};
            var content = new Dictionary<string, string>
            {
                {"firstName", firstName},
                {"lastName", lastName},
                {"email", email},
                //{"userType", userType.ToString()},
                {"password", password}
            };

            var url = URL + "register";
            //var content = JsonConvert.SerializeObject(newUser);
            var serializedContent = new FormUrlEncodedContent(content);
            var response = await client.PostAsync(url, serializedContent);

            if (response.IsSuccessStatusCode)
            {
                var newUser = new User
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    UserType = userType
                };
                ViewBag.User = newUser;
            }
            return RedirectToAction("Index");
        }

        private Models.WebResponse GetResponse(string url)
        {
            var webRequest = WebRequest.Create(url);
            webRequest.ContentType = "application/json; charset=utf-8";
            var response = (HttpWebResponse)webRequest.GetResponse();
            var dataStream = response.GetResponseStream();
            if (dataStream != null)
            {
                var reader = new StreamReader(dataStream);
                var serverResponse = reader.ReadToEnd();
                var json = JsonConvert.DeserializeObject<Models.WebResponse>(serverResponse);
                return json;
            }

            return null;
        }
    }
}
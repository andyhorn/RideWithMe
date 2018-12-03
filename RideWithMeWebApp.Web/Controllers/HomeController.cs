using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;
using RideWithMeWebApp.Models.Classes;
using RideWithMeWebApp.Models.Interfaces;
using WebRequest = System.Net.WebRequest;

namespace RideWithMeWebApp.Web.Controllers
{
    public class HomeController : Controller
    {

        private static readonly HttpClient Client = new HttpClient();
        private const string ApiUrl = "https://ridewithmeapp.azurewebsites.net/api/";
        //private string ApiUrl = ConfigurationManager.ConnectionStrings["apiConnection"].ConnectionString;

        public ActionResult Index()
        {
            ViewBag.User = Session["user"];
            return View();
        }

        public ActionResult Login()
        {
            if (Session["user"] == null)
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }


        public ActionResult Register()
        {
            if (Session["user"] != null)
            {
                return View("Index");
            }
            return View();
        }

        public ActionResult Account()
        {
            if (Session["user"] != null)
                return View();
            return View("Login");
        }
        public ActionResult SubmitLogin()
        {
            var email = Request.QueryString["Email"];
            var password = Request.QueryString["Password"];
            ClearQueryString();

            var result = GetResponse(ApiUrl + $"login?email={email}&password={password}");
            if (result != null && result.Message == "Success!")
            {
                var user = JsonConvert.DeserializeObject<User>(result.Data["user"].ToString());
                //ViewBag.Status = 0;
                //ViewBag.User = user;
                Session["user"] = user;
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

            var content = new Dictionary<string, string>
            {
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Email", email},
                {"UserType", userType.ToString()},
                {"Password", password}
            };

            var url = ApiUrl + "register";
            var serializedContent = new FormUrlEncodedContent(content);
            var response = await Client.PostAsync(url, serializedContent);

            if (response.IsSuccessStatusCode)
            {
                return SubmitLogin();
            }
            return View("Register");
        }

        public async Task<ActionResult> UpdateUser()
        {
            var email = Request.QueryString["Email"];
            var password = Request.QueryString["Password"];
            var firstName = Request.QueryString["FirstName"];
            var lastName = Request.QueryString["LastName"];
            //var success = false;
            var userId = (Session["user"] as IUser)?.Id;

            var content = new List<Dictionary<string, string>>();

            if (!string.IsNullOrEmpty(email))
            {
                content.Add(new Dictionary<string, string>()
                {
                    { "TargetId", userId.ToString() },
                    { "TargetTable", "Users" },
                    { "Param", "Email" },
                    { "NewValue", email }
                });
            }

            if (!string.IsNullOrEmpty(password))
            {
                content.Add(new Dictionary<string, string>()
                {
                    { "TargetId", userId.ToString() },
                    { "TargetTable", "Logins" },
                    { "Param", "Password" },
                    { "NewValue", password }
                });
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                content.Add(new Dictionary<string, string>()
                {
                    { "TargetId", userId.ToString() },
                    { "TargetTable", "Users" },
                    { "Param", "FirstName" },
                    { "NewValue", firstName }
                });
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                content.Add(new Dictionary<string, string>()
                {
                    { "TargetId", userId.ToString() },
                    { "TargetTable", "Users" },
                    { "Param", "LastName" },
                    { "NewValue", lastName }
                });
            }

            try
            {
                //bool success = true;
                //content.ForEach(async req => await UpdateHandler(ApiUrl + "update", req));

                var results = new Dictionary<string, bool>();
                foreach (var req in content)
                {
                    var success = await UpdateHandler(ApiUrl + "update", req);
                    results.Add(req["Param"], success);
                    if (success)
                        UpdateCurrentUser(req["Param"], req["NewValue"]);
                }

                ViewBag.Updated = true;
                ViewBag.Success = true;
                ViewBag.Results = results;
                return View("Account");
            }
            catch (Exception)
            {
                ViewBag.Success = false;
            }

            return View("Account");
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

        private async Task<bool> UpdateHandler(string url, Dictionary<string, string> content)
        {
            var serializedContent = new FormUrlEncodedContent(content);
            var response = await Client.PostAsync(url, serializedContent);

            return response.IsSuccessStatusCode;
        }

        private void UpdateCurrentUser(string param, string value)
        {
            var user = Session["user"] as IUser;

            if (user != null)
            {
                switch (param)
                {
                    case "FirstName":
                        user.FirstName = value;
                        break;
                    case "LastName":
                        user.LastName = value;
                        break;
                    case "Email":
                        user.Email = value;
                        break;
                }
            }

            Session["user"] = user;
        }

        private void ClearQueryString()
        {
            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty(
                "IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);

            isreadonly.SetValue(this.Request.QueryString, false, null);

            this.Request.QueryString.Clear();
        }
    }
}
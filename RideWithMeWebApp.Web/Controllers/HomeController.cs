using System;
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
            // TODO: Convert this form submission to POST
            // Use the FormCollection collection syntax to access parameters
            // in a similar way to Request.QueryString["param"].

            // TODO: STOP USING WEBRESPONSE OBJECTS!
            // Make this method return the objects themselves.

            var email = Request.QueryString["Email"];
            var password = Request.QueryString["Password"];

            var result = GetResponse(ApiUrl + $"login?email={email}&password={password}");
            if (result != null && result.Message == "Success!")
            {
                var user = JsonConvert.DeserializeObject<User>(result.Data["user"].ToString());
                Session["user"] = user;
                return RedirectToAction("Index");
            }

            ViewBag.Status = -1;
            return RedirectToAction("Login");
        }

        public async Task<ActionResult> RegisterUser()
        {
            // TODO: Convert this method to POST with FormCollection
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
            // TODO: Convert this method to POST with FormCollection
            var email = Request.QueryString["Email"];
            var password = Request.QueryString["Password"];
            var firstName = Request.QueryString["FirstName"];
            var lastName = Request.QueryString["LastName"];
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
    }
}
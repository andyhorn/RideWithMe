using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using RideWithMeWebApp.Models.Classes;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.Web.Controllers
{
    public class UsersController : Controller
    {
        private static readonly HttpClient Client = new HttpClient();

        //private string ApiUrl = ConfigurationManager.ConnectionStrings["ApiConnection"].ConnectionString;
        private const string ApiUrl = "https://ridewithmeapp.azurewebsites.net/api/";
        // GET: Users
        public async Task<ActionResult> Index()
        {
            if (Session["user"] != null && ((IUser) Session["user"]).UserType == 2)
            {
                ViewBag.Users = await GetAllUsers();
                return View();
            }

            return RedirectToRoute("Root");
        }

        private async Task<List<User>> GetAllUsers()
        {
            var url = ApiUrl + "admin/users/all";
            var response = await Client.GetAsync(url);
            var responseData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<User>>(responseData);
            return data;
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (Session["user"] == null || id == null) return RedirectToAction("Index");

            var user = (IUser) Session["user"];

            if (user.UserType != 2) return RedirectToAction("Index");

            var url = ApiUrl + $"admin/users/{id.ToString()}";
            var response = await Client.GetAsync(url);
            var responseData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<User>(responseData);
            return View(data);
        }

        public async Task<ActionResult> UpdateUser(FormCollection collection)
        {
            var email = collection["Email"];
            var password = collection["Password"];
            var firstName = collection["FirstName"];
            var lastName = collection["LastName"];
            var status = collection["Status"];
            var userId = ((IUser) Session["editUser"]).Id;

            if (Session["user"] == null || ((IUser) Session["user"]).UserType != 2)
                return RedirectToRoute("Root");

            var content = new List<Dictionary<string, string>>();

            if (!string.IsNullOrEmpty(email))
            {
                content.Add(new Dictionary<string, string>
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

            if (!string.IsNullOrEmpty(status))
            {
                content.Add(new Dictionary<string, string>
                {
                    { "TargetId", userId.ToString() },
                    { "TargetTable", "Users" },
                    { "Param", "UserType" },
                    { "NewValue", status }
                });
            }

            // LINQ statement won't work with async and await, keep getting exceptions.
            foreach (var request in content)
                await UpdateHandler(ApiUrl + "update", request);

            return RedirectToAction("Index");
        }

        private async Task<bool> UpdateHandler(string url, Dictionary<string, string> content)
        {
            var serializedContent = new FormUrlEncodedContent(content);
            var response = await Client.PostAsync(url, serializedContent);

            return response.IsSuccessStatusCode;
        }
    }
}
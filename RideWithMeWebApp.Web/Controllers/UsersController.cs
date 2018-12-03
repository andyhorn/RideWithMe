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
            if (Session["user"] != null && (Session["user"] as IUser).UserType == 2)
            {
                ViewBag.Users = await GetAllUsers();
                return View();
            }

            return RedirectToAction("Login");
        }

        private async Task<List<User>> GetAllUsers()
        {
            var url = ApiUrl + "admin/users/all";
            var response = await Client.GetAsync(url);
            var responseData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<User>>(responseData);
            return data;
        }

        public async Task<ActionResult> EditUser(int id)
        {
            return null;
        }
    }
}
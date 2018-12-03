using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using RideWithMeWebApp.Models.Classes;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.Web.Controllers
{
    public class RidesController : Controller
    {
        private const string ApiUrl = "https://ridewithmeapp.azurewebsites.net/api/";
        private static readonly HttpClient Client = new HttpClient();

        // GET: Rides
        [HttpGet()]
        public async Task<ActionResult> Index()
        {
            if (Session["user"] != null)
            {
                var user = Session["user"] as IUser;
                var collection = new List<Ride>();
                if (user.UserType == 0) // Riders
                {
                    collection = await GetRidesByRiderId(user.Id);
                }
                else if (user.UserType == 1) // Drivers
                {
                    collection = await GetRidesByDriverId(user.Id);
                }
                else if (user.UserType == 2) // Administrators
                {
                    collection = await GetAllRides();
                }

                ViewBag.Rides = collection;
                return View();
            }

            return RedirectToAction("Login");
        }

        private async Task<List<Ride>> GetRidesByRiderId(long id)
        {
            var url = ApiUrl + $"rides/riderId/{id.ToString()}";
            var response = await Client.GetAsync(url);
            var responseData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Ride>>(responseData);
            return data;
        }

        private async Task<List<Ride>> GetRidesByDriverId(long id)
        {
            var url = ApiUrl + $"rides/DriverId/{id.ToString()}";
            var response = await Client.GetAsync(url);
            var responseData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Ride>>(responseData);
            return data;
        }

        private async Task<List<Ride>> GetRideById(long id)
        {
            var url = ApiUrl + $"rides/rideId/{id}";
            var response = await Client.GetAsync(url);
            var responseData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Ride>>(responseData);
            return data;
        }

        private async Task<List<Ride>> GetAllRides()
        {
            var url = ApiUrl + $"admin/rides/all";
            var response = await Client.GetAsync(url);
            var responseData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Ride>>(responseData);
            return data;
        }
    }
}
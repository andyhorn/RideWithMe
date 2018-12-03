using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RideWithMeWebApp.Models.Classes;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.Web.Controllers
{
    public class VehiclesController : Controller
    {
        private const string ApiUrl = "https://ridewithmeapp.azurewebsites.net/api/";
        private static readonly HttpClient Client = new HttpClient();
        // GET: Vehicles
        public async Task<ActionResult> Index()
        {
            if (Session["user"] != null)
            {
                var user = Session["user"] as IUser;
                if (user.UserType == 0) // Riders
                {
                    return RedirectToAction("Index");
                }
                else if (user.UserType == 1) // Drivers
                {
                    List<Vehicle> vehicles = await GetVehiclesForUser(user.Id);
                    ViewBag.Vehicles = vehicles;
                }
                else if (user.UserType == 2) // Admin
                {
                    List<Vehicle> vehicles = await GetAllVehicles();
                    ViewBag.Vehicles = vehicles;
                }

                return View();
            }

            return RedirectToAction("Login");
        }

        private async Task<List<Vehicle>> GetVehiclesForUser(long userId)
        {
            var url = ApiUrl + $"vehicles/user/{userId}";
            var response = await Client.GetAsync(url);
            var responseData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Vehicle>>(responseData);
            return data;
        }

        private async Task<List<Vehicle>> GetAllVehicles()
        {
            var url = ApiUrl + "admin/vehicles/all";
            var response = await Client.GetAsync(url);
            var responseData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Vehicle>>(responseData);
            return data;
        }
    }
}
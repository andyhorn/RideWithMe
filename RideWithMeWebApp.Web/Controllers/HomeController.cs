using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RideWithMeWebApp.Models.Classes;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUser _currentUser;

        public HomeController()
        {
            _currentUser = new User();
        }
        public ActionResult Index()
        {
            _currentUser.UserType = 0;
            ViewData["user"] = _currentUser;
            return View(_currentUser);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
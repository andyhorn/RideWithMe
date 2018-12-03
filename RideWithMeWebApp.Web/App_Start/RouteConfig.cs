using System.Web.Mvc;
using System.Web.Routing;

namespace RideWithMeWebApp.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Root",
                "",
                new { controller = "Home", action = "Index" });
            routes.MapRoute(
                "Rides",
                "rides/{action}",
                new { controller = "Rides", action = "Index" });
            routes.MapRoute(
                "Users",
                "users/{action}",
                new { controller = "Users", action = "Index" });
            routes.MapRoute(
                "Vehicles",
                "vehicles/{action}",
                new { controller = "Vehicles", action = "Index" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

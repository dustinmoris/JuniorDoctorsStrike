using System.Web.Mvc;
using System.Web.Routing;

namespace JuniorDoctorsStrike.Web.Configuration
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Make all URLs lower case
            routes.LowercaseUrls = true;

            routes.MapRoute(
                name: "Index",
                url: "",
                defaults: new { controller = "Home", action = "Index", page = 1 }
            );

            routes.MapRoute(
                name: "About",
                url: "About",
                defaults: new { controller = "Home", action = "About" }
            );
        }
    }
}

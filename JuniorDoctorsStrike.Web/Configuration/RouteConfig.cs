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
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "About",
                url: "About",
                defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                name: "Stories",
                url: "Stories",
                defaults: new { controller = "Home", action = "Stories" }
            );

            routes.MapRoute(
                name: "Messages",
                url: "Api/Messages",
                defaults: new { controller = "Api", action = "Messages" }
            );

            routes.MapRoute(
                name: "MessagesSince",
                url: "Api/MessagesSince/{sinceId}",
                defaults: new { controller = "Api", action = "MessagesSince" }
            );
        }
    }
}

using System;
using System.Web.Mvc;
using System.Web.Routing;
using JuniorDoctorsStrike.Web.Configuration;

namespace JuniorDoctorsStrike.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DependencyConfig.Setup();

            // Disable HTTP headers to disclose ASP.NET MVC on the server
            MvcHandler.DisableMvcResponseHeader = true;
        }
    }
}
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(JuniorDoctorsStrike.Web.Startup))]

namespace JuniorDoctorsStrike.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}

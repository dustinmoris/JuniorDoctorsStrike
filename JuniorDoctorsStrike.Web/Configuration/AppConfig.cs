using System.Web.Configuration;

namespace JuniorDoctorsStrike.Web.Configuration
{
    public static class AppConfig
    {
        public static bool IsProduction => bool.Parse(WebConfigurationManager.AppSettings["IsProduction"]);
        public static string GoogleAnalyticsTrackingCode => WebConfigurationManager.AppSettings["GoogleAnalytics_TrackingCode"];

        public static bool IsDebugMode()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
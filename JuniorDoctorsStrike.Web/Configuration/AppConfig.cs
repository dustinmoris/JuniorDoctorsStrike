using System;
using System.Collections.Generic;
using System.Linq;
using WebConfig = System.Web.Configuration.WebConfigurationManager;

namespace JuniorDoctorsStrike.Web.Configuration
{
    public static class AppConfig
    {
        public static bool IsProduction => bool.Parse(WebConfig.AppSettings["IsProduction"]);
        public static string GoogleAnalyticsTrackingCode => WebConfig.AppSettings["GoogleAnalytics_TrackingCode"];

        public static IEnumerable<string> GetHashtags()
        {
            var value = WebConfig.AppSettings["Hashtags"];
            var hashtags = value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            return hashtags.Select(x => x.Trim());
        } 

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
using System.Configuration;

namespace JuniorDoctorsStrike.TwitterApi
{
    public class TwitterApiConfiguration : ITwitterApiConfiguration
    {
        public string BaseUrl => ConfigurationManager.AppSettings["TwitterApi_BaseUrl"];
        public string AccessToken => ConfigurationManager.AppSettings["TwitterApi_AccessToken"];
    }
}
using System.Net;

namespace JuniorDoctorsStrike.Common
{
    public class JsonSerializer
    {
        public static dynamic Deserialize(string json)
        {
            return new
            {
                Name = "Test",
                Age = "Test"
            };
        }
    }

    public class UrlEncoder : IUrlEncoder
    {
        public string Encode(string value)
        {
            return WebUtility.UrlEncode(value);
        }
    }
}
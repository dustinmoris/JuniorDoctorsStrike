using System.Net;

namespace JuniorDoctorsStrike.Common
{
    public class UrlEncoder : IUrlEncoder
    {
        public string Encode(string value)
        {
            return WebUtility.UrlEncode(value);
        }
    }
}
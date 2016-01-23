using System.Collections.Generic;

namespace JuniorDoctorsStrike.TwitterApi
{
    public interface ITwitterHashtagParser
    {
        string ConvertHashtagsToLinks(string input, IEnumerable<string> hashtags);
    }
}
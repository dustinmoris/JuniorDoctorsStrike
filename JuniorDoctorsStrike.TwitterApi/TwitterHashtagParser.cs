using System.Collections.Generic;
using System.Linq;

namespace JuniorDoctorsStrike.TwitterApi
{
    public class TwitterHashtagParser : ITwitterHashtagParser
    {
        public string ConvertHashtagsToLinks(string input, IEnumerable<string> hashtags)
        {
            return hashtags.Aggregate(input, (current, hashtag) => current.Replace($"#{hashtag}", $"<a href='https://twitter.com/search?q=%23{hashtag}'>#{hashtag}</a>"));
        }
    }
}
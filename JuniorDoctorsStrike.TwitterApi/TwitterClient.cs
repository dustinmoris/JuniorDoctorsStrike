using JuniorDoctorsStrike.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JuniorDoctorsStrike.TwitterApi
{
    public class TwitterClient : ITwitterClient
    {
        private readonly ITwitterApiConfiguration _apiConfiguration;
        private readonly IUrlEncoder _urlEncoder;

        public TwitterClient(
            ITwitterApiConfiguration apiConfiguration,
            IUrlEncoder urlEncoder)
        {
            _apiConfiguration = apiConfiguration;
            _urlEncoder = urlEncoder;
        }

        public async Task<IEnumerable<Tweet>> SearchRecent(params string[] values)
        {
            return await Search(values, ResultType.Recent);
        }

        public async Task<IEnumerable<Tweet>> Search(IEnumerable<string> values, ResultType resultType)
        {
            var client = CreateHttpClient();
            var query = CreateQuery(values);
            var encodedQuery = _urlEncoder.Encode(query);
            var resType = ConvertFromResultType(resultType);
            var requestUrl = $"search/tweets.json?q={encodedQuery}&result_type={resType}&count=30";
            var response = await client.GetAsync(requestUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            dynamic payload = JsonConvert.DeserializeObject(responseBody);

            return ParseTweets(payload);
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_apiConfiguration.BaseUrl)
            };

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiConfiguration.AccessToken}");

            return client;
        }

        private static string CreateQuery(IEnumerable<string> searchValues)
        {
            return $"{string.Join(" ", searchValues)} AND -filter:retweets AND -filter:replies";
        }

        private static string ConvertFromResultType(ResultType resultType)
        {
            switch (resultType)
            {
                case ResultType.Recent:
                    return "recent";
                case ResultType.Popular:
                    return "popular";
                case ResultType.Mixed:
                    return "mixed";
                default:
                    throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null);
            }
        }

        private static IEnumerable<Tweet> ParseTweets(dynamic payload)
        {
            foreach (var tweet in payload.statuses)
            {
                var created = DateTime.ParseExact(
                    tweet.created_at.ToString(),
                    "ddd MMM dd HH:mm:ss zzz yyyy",
                    CultureInfo.InvariantCulture);
                
                var text = tweet.text.ToString();

                text = ConvertUrlsToLinks(text);

                // Convert hashtags into links
                foreach (var hashtag in tweet.entities.hashtags)
                {
                    text = text.Replace($"#{hashtag.text}", $"<a href='https://twitter.com/search?q=%23{hashtag.text}'>#{hashtag.text}</a>");
                }

                yield return new Tweet
                {
                    Text = text,
                    Created = created,
                    TimeSinceCreated = DateTime.Now - created,
                    User = new User
                    {
                         Name = tweet.user.name,
                         ProfilePictureUrl = tweet.user.profile_image_url
                    }
                };
            }
        }

        private static string ConvertUrlsToLinks(string text)
        {
            const string pattern = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.Replace(text, "<a href=\"$1\">$1</a>").Replace("href=\"www", "href=\"http://www");
        }

        private async Task<bool> IsImageUrl(string url)
        {
            var request = WebRequest.Create(url);
            request.Method = "HEAD";

            using (var response = await request.GetResponseAsync())
            {
                return response.ContentType.ToLower(CultureInfo.InvariantCulture).StartsWith("image/");
            }
        }
    }
}

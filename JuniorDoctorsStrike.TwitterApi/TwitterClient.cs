using JuniorDoctorsStrike.Common;
using JuniorDoctorsStrike.Common.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace JuniorDoctorsStrike.TwitterApi
{
    public class TwitterClient : ITwitterClient
    {
        private readonly ITwitterApiConfiguration _configuration;
        private readonly IUrlEncoder _urlEncoder;
        private readonly IHtmlLinkParser _htmlLinkParser;
        private readonly ITwitterHashtagParser _hashtagParser;

        public TwitterClient(
            ITwitterApiConfiguration configuration,
            IUrlEncoder urlEncoder,
            IHtmlLinkParser htmlLinkParser,
            ITwitterHashtagParser hashtagParser)
        {
            _configuration = configuration;
            _urlEncoder = urlEncoder;
            _htmlLinkParser = htmlLinkParser;
            _hashtagParser = hashtagParser;
        }

        public async Task<IEnumerable<Message>> SearchAsync(IEnumerable<string> values, ResultType resultType)
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
                BaseAddress = new Uri(_configuration.BaseUrl)
            };

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration.AccessToken}");

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

        private IEnumerable<Message> ParseTweets(dynamic payload)
        {
            foreach (var tweet in payload.statuses)
            {
                var created = DateTime.ParseExact(
                    tweet.created_at.ToString(),
                    "ddd MMM dd HH:mm:ss zzz yyyy",
                    CultureInfo.InvariantCulture);

                var hashtags = new List<string>();

                foreach (var hashtag in tweet.entities.hashtags)
                {
                    hashtags.Add(hashtag.text.ToString());
                }

                var rawText = tweet.text.ToString();
                var parsedText = _htmlLinkParser.ParseHtmlLinks(rawText);
                var parsedTextWithHashtags = _hashtagParser.ConvertHashtagsToLinks(parsedText, hashtags);

                yield return new Message
                {
                    Text = parsedTextWithHashtags,
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
    }
}

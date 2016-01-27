using JuniorDoctorsStrike.Common;
using JuniorDoctorsStrike.TwitterApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorDoctorsStrike.Core
{
    public class MessageService : IMessageService
    {
        private const int Count = 30;

        private readonly ITwitterClient _twitterClient;
        private readonly IEnumerable<string> _hashtags; 

        public MessageService(
            ITwitterClient twitterClient, 
            IEnumerable<string> hashtags)
        {
            _twitterClient = twitterClient;
            _hashtags = hashtags;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _twitterClient.SearchAsync(_hashtags, ResultType.Recent, Count).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Message>> GetMessagesSinceAsync(long sinceId)
        {
            return await _twitterClient.SearchAsync(
                _hashtags, 
                ResultType.Recent, 
                ResultTime.SinceId, 
                Count, 
                sinceId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Message>> GetMessagesUntilAsync(long maxId)
        {
            return await _twitterClient.SearchAsync(
                _hashtags,
                ResultType.Recent,
                ResultTime.MaxId,
                Count,
                maxId).ConfigureAwait(false);
        }
    }
}

using JuniorDoctorsStrike.Common;
using JuniorDoctorsStrike.TwitterApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorDoctorsStrike.Core
{
    //public class CachedMessage

    public class MessageService : IMessageService
    {
        private const int Count = 30;

        private readonly ITwitterClient _twitterClient;
        private readonly IEnumerable<string> _hashtagsToObserve; 

        public MessageService(ITwitterClient twitterClient)
        {
            _twitterClient = twitterClient;

            _hashtagsToObserve = new []
            {
                "#JuniorDoctorsStrike",
                "#juniorcontract",
                "#notsafenotfair",
                "#SaveOurNHS"
            };
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _twitterClient.SearchAsync(_hashtagsToObserve, ResultType.Recent, Count).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Message>> GetMessagesSinceAsync(long sinceId)
        {
            return await _twitterClient.SearchAsync(
                _hashtagsToObserve, 
                ResultType.Recent, 
                ResultTime.SinceId, 
                Count, 
                sinceId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Message>> GetMessagesUntilAsync(long maxId)
        {
            return await _twitterClient.SearchAsync(
                _hashtagsToObserve,
                ResultType.Recent,
                ResultTime.MaxId,
                Count,
                maxId).ConfigureAwait(false);
        }
    }
}

using JuniorDoctorsStrike.Common;
using JuniorDoctorsStrike.TwitterApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorDoctorsStrike.Core
{
    public class StatusUpdateService : IStatusUpdateService
    {
        private readonly ITwitterClient _twitterClient;
        private readonly IEnumerable<string> _hashtagsToObserve; 

        public StatusUpdateService(ITwitterClient twitterClient)
        {
            _twitterClient = twitterClient;

            _hashtagsToObserve = new []
            {
                "#JuniorDoctorsStrike"
            };
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _twitterClient.SearchAsync(_hashtagsToObserve, ResultType.Recent).ConfigureAwait(false);
        } 
    }
}

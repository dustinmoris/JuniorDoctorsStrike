using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JuniorDoctorsStrike.Common;
using JuniorDoctorsStrike.Common.Web;
using JuniorDoctorsStrike.TwitterApi;

namespace JuniorDoctorsStrike.Core
{
    public class StatusUpdateTicker
    {
        private readonly IMessagesService _messagesService;
        private readonly object _padlock = new object();
        private IEnumerable<Message> _messages = new List<Message>();

        private static readonly Lazy<StatusUpdateTicker> _instance = new Lazy<StatusUpdateTicker>(CreateInstance);
        public static StatusUpdateTicker Instance => _instance.Value;

        private static StatusUpdateTicker CreateInstance()
        {
            return new StatusUpdateTicker(
                new MessagesService(
                    new TwitterClient(
                        new TwitterApiConfiguration(),
                        new UrlEncoder(),
                        new HtmlLinkParser(),
                        new TwitterHashtagParser())), 30);
        }

        private StatusUpdateTicker(
            IMessagesService messagesService, 
            int intervalInSeconds)
        {
            _messagesService = messagesService;
            var interval = TimeSpan.FromSeconds(intervalInSeconds);

            Task.Run(InitMessagesAsync).Wait();

            while (true)
            {
                Task.Run(UpdateMessages).Wait();
                Task.Delay(interval).Wait();
            }
        }

        private async Task InitMessagesAsync()
        {
            var messages = await _messagesService.GetMessagesAsync();
            SetMessages(messages);
        }

        private async Task UpdateMessages()
        {
            var sinceId = _messages.First().Id;
            var messages = await _messagesService.GetMessagesAsync(sinceId);
            var newMessages = messages.Except(_messages).ToList();

            if (newMessages.Any())
            {
                var merged = newMessages.Union(_messages);
                var updated = merged.Take(30);
                SetMessages(updated);

                // Notify
            }
        }

        private void SetMessages(IEnumerable<Message> messages)
        {
            lock (_padlock)
            {
                _messages = messages;
            }
        }

        public IEnumerable<Message> GetMessages()
        {
            return _messages;
        }

        public void Register(Action<IEnumerable<Message>> updateMessages)
        {
            throw new NotImplementedException();
        }
    }
}
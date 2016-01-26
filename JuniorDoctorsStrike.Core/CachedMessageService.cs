using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JuniorDoctorsStrike.Common;

namespace JuniorDoctorsStrike.Core
{
    public class CachedMessageService : IMessageService
    {
        private const int Count = 30;

        private readonly IMessageService _messageService;

        //private static Lazy<IMessageService> _instance = new Lazy<IMessageService>(new CachedMessageService(CreateBaseMessageService.Invoke()) as IMessageService);
        private IEnumerable<Message> MessageCache = new List<Message>();
        private readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(5);

        public static Func<IMessageService> CreateBaseMessageService;

        private CachedMessageService(IMessageService messageService)
        {
            _messageService = messageService;

            SetInitialSetOfMessages();

            InitializeMessagePolling();
        }

        private void SetInitialSetOfMessages()
        {
            var initialSetOfMessages = _messageService.GetMessagesAsync().Result;
            MessageCache = initialSetOfMessages;
        }

        private void InitializeMessagePolling()
        {
            new Timer(PollMessages, null, PollingInterval, PollingInterval);
        }

        private object _padlock;

        private void PollMessages(object state)
        {
            lock (_padlock)
            {
                var copyOfCache = MessageCache;
                var sinceId = copyOfCache.OrderByDescending(x => x.Created).First().Id;
                var messageService = CreateBaseMessageService.Invoke();
                var messages = messageService.GetMessagesSinceAsync(sinceId).Result;


            }

            foreach (var message in messages.Where(message => MessageCache.All(x => x.Id != message.Id)))
            {
                MessageCache.Add(message);
            }

            if (MessageCache.Count > 300)
            {
                //MessageCache = MessageCache.OrderByDescending(x => x.Created).Take(300);
            }
        }

        public Task<IEnumerable<Message>> GetMessagesAsync()
        {
            var copyOfMessages = MessageCache;

            return Task.FromResult(
                copyOfMessages
                    .OrderByDescending(x => x.Created)
                    .Take(Count));
        }

        public Task<IEnumerable<Message>> GetMessagesSinceAsync(long sinceId)
        {
            var copyOfMessages = MessageCache;

            return Task.FromResult(
                copyOfMessages
                    .Where(x => x.Id > sinceId)
                    .OrderByDescending(x => x.Created) 
                    as IEnumerable<Message>);
        }

        public Task<IEnumerable<Message>> GetMessagesUntilAsync(long maxId)
        {
            return _messageService.GetMessagesUntilAsync(maxId);
        }
    }
}
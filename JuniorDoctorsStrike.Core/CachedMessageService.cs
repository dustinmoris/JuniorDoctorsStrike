using JuniorDoctorsStrike.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JuniorDoctorsStrike.Core
{
    public sealed class CachedMessageService : IMessageService
    {
        private const int Count = 30;
        private const int PollingIntervalInSeconds = 5;
        private const int MaxCacheCount = 1000;

        private readonly IMessageService _messageService;
        private readonly TimeSpan _pollingInterval = TimeSpan.FromSeconds(PollingIntervalInSeconds);
        private readonly object _padlock = new object();

        private Timer _messagePollingTicker;
        private volatile HashSet<Message> _messageCache = new HashSet<Message>();

        public static Func<IMessageService> CreateBaseMessageService;

        private static readonly Lazy<CachedMessageService> _instance = 
            new Lazy<CachedMessageService>(() => new CachedMessageService(CreateBaseMessageService.Invoke()));

        public static CachedMessageService Instance => _instance.Value;

        private CachedMessageService(IMessageService messageService)
        {
            _messageService = messageService;
            SetInitialSetOfMessages();
            InitializeMessagePolling();
        }

        private void SetInitialSetOfMessages()
        {
            var initialSetOfMessages = _messageService.GetMessagesAsync().Result;

            foreach (var message in initialSetOfMessages)
            {
                _messageCache.Add(message);
            }
        }

        private void InitializeMessagePolling()
        {
            _messagePollingTicker = new Timer(PollMessages, null, _pollingInterval, _pollingInterval);
        }

        private void PollMessages(object state)
        {
            var copyOfCachedMessages = new HashSet<Message>(_messageCache);
            var sinceId = copyOfCachedMessages.OrderByDescending(x => x.Created).First().IdValue;
            var messageService = CreateBaseMessageService.Invoke();
            var messages = messageService.GetMessagesSinceAsync(sinceId).Result.ToList();

            if (messages.Count == 0)
                return;

            foreach (var message in messages)
            {
                copyOfCachedMessages.Add(message);
            }

            if (copyOfCachedMessages.Count > MaxCacheCount)
            {
                var reducedCachedMessages = 
                    copyOfCachedMessages
                        .OrderByDescending(x => x.Created)
                        .Take(MaxCacheCount);

                copyOfCachedMessages.Clear();

                foreach (var message in reducedCachedMessages)
                {
                    copyOfCachedMessages.Add(message);
                }
            }

            lock (_padlock)
            {
                _messageCache = copyOfCachedMessages;
            }
        }

        public Task<IEnumerable<Message>> GetMessagesAsync()
        {
            var copyOfCachedMessages = new HashSet<Message>(_messageCache);

            return Task.FromResult(
                copyOfCachedMessages
                    .OrderByDescending(x => x.Created)
                    .Take(Count));
        }

        public Task<IEnumerable<Message>> GetMessagesSinceAsync(long sinceId)
        {
            var copyOfCachedMessages = new HashSet<Message>(_messageCache);

            return Task.FromResult(
                copyOfCachedMessages
                    .Where(x => x.IdValue > sinceId)
                    .OrderByDescending(x => x.Created) 
                    as IEnumerable<Message>);
        }

        public async Task<IEnumerable<Message>> GetMessagesUntilAsync(long maxId)
        {
            var copyOfCachedMessages = new HashSet<Message>(_messageCache);

            var messages = 
                copyOfCachedMessages
                    .Where(x => x.IdValue < maxId)
                    .OrderByDescending(x => x.Created)
                    .Take(Count)
                    .ToList();

            if (messages.Count > 0)
                return messages;

            var newMessages = await _messageService.GetMessagesUntilAsync(maxId);

            foreach (var message in newMessages)
            {
                copyOfCachedMessages.Add(message);
            }

            lock (_padlock)
            {
                _messageCache = copyOfCachedMessages;
            }

            return copyOfCachedMessages
                    .Where(x => x.IdValue < maxId)
                    .OrderByDescending(x => x.Created)
                    .Take(Count);
        }
    }
}
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

        private readonly IMessageService _messageService;
        private readonly TimeSpan _pollingInterval = TimeSpan.FromSeconds(5);
        private readonly object _padlock = new object();

        private Timer _messagePollingTicker;
        private volatile List<Message> _messageCache = new List<Message>();

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
            _messageCache = initialSetOfMessages.ToList();
        }

        private void InitializeMessagePolling()
        {
            _messagePollingTicker = new Timer(PollMessages, null, _pollingInterval, _pollingInterval);
        }

        private void PollMessages(object state)
        {
            var copyOfCachedMessages = new List<Message>(_messageCache);
            var sinceId = copyOfCachedMessages.OrderByDescending(x => x.Created).First().Id;
            var messageService = CreateBaseMessageService.Invoke();
            var messages = messageService.GetMessagesSinceAsync(sinceId).Result;

            var temp = copyOfCachedMessages;
            copyOfCachedMessages.AddRange(messages.Where(message => !temp.Any(m => m.Id.Equals(message.Id))));

            if (copyOfCachedMessages.Count > 300)
            {
                copyOfCachedMessages = 
                    copyOfCachedMessages
                        .OrderByDescending(x => x.Created)
                        .Take(300)
                        .ToList();
            }

            lock (_padlock)
            {
                _messageCache = copyOfCachedMessages;
            }
        }

        public Task<IEnumerable<Message>> GetMessagesAsync()
        {
            var copyOfCachedMessages = new List<Message>(_messageCache);

            return Task.FromResult(
                copyOfCachedMessages
                    .OrderByDescending(x => x.Created)
                    .Take(Count));
        }

        public Task<IEnumerable<Message>> GetMessagesSinceAsync(long sinceId)
        {
            var copyOfCachedMessages = new List<Message>(_messageCache);

            return Task.FromResult(
                copyOfCachedMessages
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
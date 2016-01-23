using JuniorDoctorsStrike.Common;
using JuniorDoctorsStrike.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;

namespace JuniorDoctorsStrike.Web.Hubs
{
    [HubName("messages")]
    public class MessagesHub : Hub
    {
        private readonly StatusUpdateTicker _statusUpdateTicker;

        public MessagesHub() : this(StatusUpdateTicker.Instance)
        {
        }

        public MessagesHub(StatusUpdateTicker statusUpdateTicker)
        {
            _statusUpdateTicker = statusUpdateTicker;

            _statusUpdateTicker.Register(BroadcastMessages);
        }

        public IEnumerable<Message> GetMessages()
        {
            return _statusUpdateTicker.GetMessages();
        }

        public void BroadcastMessages(IEnumerable<Message> messages)
        {
            Clients.All.updateMessages(messages);
        }
    }
}
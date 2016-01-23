using JuniorDoctorsStrike.Common;
using JuniorDoctorsStrike.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorDoctorsStrike.Web.Hubs
{
    public class StatusUpdateTicker
    {
        private readonly IStatusUpdateService _statusUpdateService;
        private IEnumerable<Message> _messages;
        private object _padlock;

        private StatusUpdateTicker(IStatusUpdateService statusUpdateService)
        {
            _statusUpdateService = statusUpdateService;
        }

        //public static async Task<StatusUpdateTicker> Create()
        //{
        //    var intervall = TimeSpan.FromSeconds(30);

        //    while (true)
        //    {
        //        await Task.Run(() => UpdateStockPrices());
        //        await Task.Delay(intervall);
        //    }
        //}

        //private async Task UpdateStockPrices()
        //{
        //    var messages = await _statusUpdateService.GetMessagesAsync();
        //    var merged = _messages.Union(messages);
        //    var mostRecent = merged.Take(30);

        //    if (
        //        )
        //    lock (_padlock)
        //    {
        //        _messages = mostRecent;
        //    }

        //    return null;
        //}
    }

    [HubName("statusUpdate")]
    public class StatusUpdateHub : Hub
    {
        public void Hello()
        {
            //Clients.All.hello();
        }
    }
}
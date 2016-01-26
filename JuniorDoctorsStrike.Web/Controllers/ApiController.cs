using System.Threading.Tasks;
using System.Web.Mvc;
using JuniorDoctorsStrike.Core;

namespace JuniorDoctorsStrike.Web.Controllers
{
    public class ApiController : Controller
    {
        private readonly IMessageService _messageService;

        public ApiController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<ActionResult> Messages()
        {
            var messages = await _messageService.GetMessagesAsync();

            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> MessagesSince(long sinceId)
        {
            var messages = await _messageService.GetMessagesSinceAsync(sinceId);

            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> MessagesUntil(long maxId)
        {
            var messages = await _messageService.GetMessagesUntilAsync(maxId);

            return Json(messages, JsonRequestBehavior.AllowGet);
        }
    }
}
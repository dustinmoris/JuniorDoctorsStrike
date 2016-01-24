using System.Threading.Tasks;
using System.Web.Mvc;
using JuniorDoctorsStrike.Core;

namespace JuniorDoctorsStrike.Web.Controllers
{
    public class ApiController : Controller
    {
        private readonly IMessagesService _messagesService;

        public ApiController(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        public async Task<ActionResult> Messages()
        {
            var messages = await _messagesService.GetMessagesAsync();

            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> MessagesSince(long sinceId)
        {
            var messages = await _messagesService.GetMessagesAsync(sinceId);

            return Json(messages, JsonRequestBehavior.AllowGet);
        }
    }
}
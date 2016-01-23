using JuniorDoctorsStrike.Core;
using JuniorDoctorsStrike.Web.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JuniorDoctorsStrike.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessagesService _messagesService;

        public HomeController(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        public async Task<ActionResult> Index()
        {
            var tweets = await _messagesService.GetMessagesAsync();
            var viewModel = new IndexViewModel { Tweets = tweets };
            return View(viewModel);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Stories()
        {
            return View();
        }
    }
}
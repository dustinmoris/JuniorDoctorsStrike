using JuniorDoctorsStrike.Core;
using JuniorDoctorsStrike.Web.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JuniorDoctorsStrike.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatusUpdateService _statusUpdateService;

        public HomeController(IStatusUpdateService statusUpdateService)
        {
            _statusUpdateService = statusUpdateService;
        }

        public async Task<ActionResult> Index()
        {
            var tweets = await _statusUpdateService.GetMessagesAsync();
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
using JuniorDoctorsStrike.TwitterApi;
using JuniorDoctorsStrike.Web.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JuniorDoctorsStrike.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITwitterClient _twitterClient;

        public HomeController(ITwitterClient twitterClient)
        {
            _twitterClient = twitterClient;
        }

        public async Task<ActionResult> Index()
        {
            //var tweets = await _twitterClient.SearchRecent("#JuniorDoctorsStrike");
            //var viewModel = new IndexViewModel { Tweets = tweets };
            var viewModel = new IndexViewModel();
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
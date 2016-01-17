using System.Collections.Generic;
using JuniorDoctorsStrike.TwitterApi;

namespace JuniorDoctorsStrike.Web.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Tweet> Tweets { get; set; }
    }
}
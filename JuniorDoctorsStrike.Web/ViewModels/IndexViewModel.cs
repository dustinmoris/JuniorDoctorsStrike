using System.Collections.Generic;
using JuniorDoctorsStrike.Common;
using JuniorDoctorsStrike.TwitterApi;

namespace JuniorDoctorsStrike.Web.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Message> Tweets { get; set; }
    }
}
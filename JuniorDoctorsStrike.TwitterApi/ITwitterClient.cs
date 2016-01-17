using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorDoctorsStrike.TwitterApi
{
    public interface ITwitterClient
    {
        Task<IEnumerable<Tweet>> SearchRecent(params string[] values);
        Task<IEnumerable<Tweet>> Search(IEnumerable<string> values, ResultType resultType);
    }
}
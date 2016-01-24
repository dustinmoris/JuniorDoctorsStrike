using System.Collections.Generic;
using System.Threading.Tasks;
using JuniorDoctorsStrike.Common;

namespace JuniorDoctorsStrike.TwitterApi
{
    public interface ITwitterClient
    {
        Task<IEnumerable<Message>> SearchAsync(
            IEnumerable<string> values, 
            ResultType resultType,
            int count);

        Task<IEnumerable<Message>> SearchAsync(
            IEnumerable<string> values,
            ResultType resultType,
            ResultTime resultTime,
            int count,
            long id);
    }
}
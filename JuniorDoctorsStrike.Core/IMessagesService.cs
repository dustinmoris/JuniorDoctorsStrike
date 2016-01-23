using System.Collections.Generic;
using System.Threading.Tasks;
using JuniorDoctorsStrike.Common;

namespace JuniorDoctorsStrike.Core
{
    public interface IMessagesService
    {
        Task<IEnumerable<Message>> GetMessagesAsync();
        Task<IEnumerable<Message>> GetMessagesAsync(long sinceId);
    }
}
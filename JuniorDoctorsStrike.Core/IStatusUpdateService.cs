using System.Collections.Generic;
using System.Threading.Tasks;
using JuniorDoctorsStrike.Common;

namespace JuniorDoctorsStrike.Core
{
    public interface IStatusUpdateService
    {
        Task<IEnumerable<Message>> GetMessagesAsync();
    }
}
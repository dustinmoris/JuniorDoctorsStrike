using System.Collections.Generic;
using System.Threading.Tasks;
using JuniorDoctorsStrike.Common;

namespace JuniorDoctorsStrike.Core
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessagesAsync();
        Task<IEnumerable<Message>> GetMessagesSinceAsync(long sinceId);
        Task<IEnumerable<Message>> GetMessagesUntilAsync(long maxId);
    }
}
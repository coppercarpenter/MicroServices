using PlatformService.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDto platform);
    }
}

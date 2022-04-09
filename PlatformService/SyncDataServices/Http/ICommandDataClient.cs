using Microsoft.Extensions.Logging;
using PlatformService.DTOs;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{

    public interface ICommandDataClient
    {
        Task SendPlatformToCommandAsync(PlatformReadDto platform, ILogger _logger);
    }
}

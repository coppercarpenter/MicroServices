using CommandsService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommandsService.Data
{
    public interface ICommandRepository
    {
        bool SaveChanges();
        IEnumerable<Platform> GetPlatforms();
        void CreatePlatform(Platform platform);
        bool PlatformExists(long platform_id);
        bool ExternalPlatformExists(long external_id);


        IEnumerable<Command> GetCommands(long platform_id);
        Command GetCommand(long platform_id, long id);
        void CreateCommand(long platform_id, Command command);
    }
}

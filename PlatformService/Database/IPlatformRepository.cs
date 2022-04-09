using PlatformService.Models;
using System.Collections.Generic;

namespace PlatformService.Database
{

    public interface IPlatformRepository
    {
        bool SaveChanges();
        IEnumerable<Platform> GetPlatforms();
        Platform GetPlatform(long id);
        void CreatePlatform(Platform platform);
    }
}

using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlatformService.Database
{
    internal class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext _db;
        public PlatformRepository(AppDbContext db)
        {
            _db = db;
        }
        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
                throw new ArgumentNullException(nameof(platform));

            _db.Platforms.Add(platform);
        }

        public Platform GetPlatform(long id)
        {
            return _db.Platforms.FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<Platform> GetPlatforms()
        {
            return _db.Platforms.ToList();
        }

        public bool SaveChanges()
        {
            return _db.SaveChanges() >= 0;
        }
    }
}

using CommandsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandsService.Data
{
    internal class CommandRepository : ICommandRepository
    {
        private AppDbContext _db;

        public CommandRepository(AppDbContext db)
        {
            _db = db;
        }

        public void CreateCommand(long platform_id, Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            command.Platform_Id = platform_id;

            _db.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
                throw new ArgumentNullException(nameof(platform));
            _db.Platforms.Add(platform);
        }

        public bool ExternalPlatformExists(long external_id)
        {
            return _db.Platforms.Any(a => a.External_Id == external_id);
        }

        public Command GetCommand(long platform_id, long id)
        {
            return GetCommands(platform_id).FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<Command> GetCommands(long platform_id)
        {
            return _db.Commands.Where(w => w.Platform_Id == platform_id).OrderBy(o => o.Platform.Name);
        }

        public IEnumerable<Platform> GetPlatforms()
        {
            return _db.Platforms.ToList();
        }

        public bool PlatformExists(long platform_id)
        {
            return _db.Platforms.Any(a => a.Id == platform_id);
        }

        public bool SaveChanges()
        {
            return _db.SaveChanges() > 0;
        }
    }
}

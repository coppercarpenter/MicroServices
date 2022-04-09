using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace CommandsService.Data
{
    public static class InitializeDatabase
    {
        public static void Populate(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var client = scope.ServiceProvider.GetRequiredService<IPlatformDataClient>();
                var platforms = client.ReturnAllPlatforms();
                SeedData(scope.ServiceProvider.GetRequiredService<ICommandRepository>(), platforms);
            }
        }
        private static void SeedData(ICommandRepository repo, IEnumerable<Platform> platforms)
        {
            foreach (var platform in platforms)
            {
                if (!repo.ExternalPlatformExists(platform.External_Id))
                {
                    repo.CreatePlatform(platform);
                }
                repo.SaveChanges();
            }
        }
    }
}
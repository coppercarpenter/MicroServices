using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using System;
using System.Linq;

namespace PlatformService.Database
{
    public static class InitializeDatabase
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                SeedData(db, isProduction);
            }
        }

        private static void SeedData(AppDbContext db, bool isProduction)
        {
            if (isProduction)
            {
                Console.WriteLine("--> Applying migrations");
                try
                {
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            if (!db.Platforms.Any())
            {
                db.Platforms.AddRange(
                    new Platform() { Name = "Dot net", Publisher = "Microsoft", Cost = "Free" },
                        new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                        new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing foundation", Cost = "Free" }
                );
                db.SaveChanges();
            }
        }
    }
}
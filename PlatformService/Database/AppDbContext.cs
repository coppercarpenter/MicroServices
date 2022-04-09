using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Database
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Platform> Platforms { get; set; }
    }
}

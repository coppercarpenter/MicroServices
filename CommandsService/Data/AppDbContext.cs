using CommandsService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace CommandsService.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Command> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Command>()
                   .HasOne(h => h.Platform)
                   .WithMany(w => w.Commands)
                   .HasForeignKey(h => h.Platform_Id);
        }
    }
}
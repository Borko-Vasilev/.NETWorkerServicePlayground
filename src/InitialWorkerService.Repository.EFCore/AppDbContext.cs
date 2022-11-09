using InitialWorkerService.Domain.Logs.Models;
using InitialWorkerService.Repository.EFCore.Logs.Builders;
using Microsoft.EntityFrameworkCore;

namespace InitialWorkerService.Repository.EFCore
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            LogsBuilder.Build(builder);
        }
    }
}

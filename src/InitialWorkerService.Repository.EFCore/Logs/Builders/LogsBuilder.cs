using InitialWorkerService.Domain.Logs.Models;
using Microsoft.EntityFrameworkCore;

namespace InitialWorkerService.Repository.EFCore.Logs.Builders
{
    internal static class LogsBuilder
    {
        internal static void Build(ModelBuilder builder)
        {
            builder.Entity<Log>(log =>
            {
                log.HasKey(log => log.Id);
                log.Property(show => show.Message).HasMaxLength(1000);
            });
        }
    }
}

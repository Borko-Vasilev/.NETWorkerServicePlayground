using InitialWorkerService.Domain.Logs;
using InitialWorkerService.Domain.Logs.Models;

namespace InitialWorkerService.Repository.EFCore.Logs
{
    public class LogsRepository : BaseRepository<Log>, ILogsRepository
    {
        public LogsRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}

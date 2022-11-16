using InitialWorkerService.Contracts.Logs.Models;

namespace InitialWorkerService.Contracts.Logs
{
    public interface ILogsService
    {
        Task<LogModel> Create(LogModel logModel);
    }
}

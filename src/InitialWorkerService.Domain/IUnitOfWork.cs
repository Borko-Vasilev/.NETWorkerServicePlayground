using InitialWorkerService.Domain.Logs;

namespace InitialWorkerService.Domain
{
    public interface IUnitOfWork
    {
        public ILogsRepository Logs { get; }

        Task<int> Complete();
    }
}

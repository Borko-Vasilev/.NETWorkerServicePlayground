using InitialWorkerService.Contracts.Logs;
using InitialWorkerService.Contracts.Logs.Models;
using InitialWorkerService.Domain;
using InitialWorkerService.Domain.Logs.Models;
using InitialWorkerService.Services.Logs.Mappers;

namespace InitialWorkerService.Services.Logs
{
    public class LogsService : ILogsService
    {
        private readonly IUnitOfWork unitOfWork;

        public LogsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<LogModel> Create(LogModel logModel)
        {
            Log log = LogsMapper.Map(logModel);

            await unitOfWork.Logs.Add(log);

            await unitOfWork.Complete();

            return LogsMapper.Map(log);
        }
    }
}

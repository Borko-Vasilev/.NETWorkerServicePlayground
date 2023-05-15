using InitialWorkerService.Contracts.Logs;
using InitialWorkerService.Contracts.Logs.Models;

namespace InitialWorkerService.Services
{
    public interface IWorkService
    {
        Task DoWork();
    }

    public class LogMessageService : IWorkService
    {
        protected ILogsService logsService;

        public LogMessageService(ILogsService logsService)
        {
            this.logsService = logsService;
        }

        public async Task DoWork()
        {
            LogModel log = new()
            {
                Message = $"Worker running at: {DateTime.Now}",
                CreateTime = DateTime.Now,
                LogType = EnumLogTypes.Information
            };

            await logsService.Create(log);
        }
    }
}

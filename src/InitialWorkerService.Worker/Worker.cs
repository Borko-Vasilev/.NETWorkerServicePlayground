using InitialWorkerService.Contracts.Logs;
using InitialWorkerService.Contracts.Logs.Models;

namespace InitialWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ILogsService logsService;

        public Worker(ILogger<Worker> logger, ILogsService logsService)
        {
            _logger = logger;
            this.logsService = logsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                LogModel log = new()
                {
                    Message = $"Worker running at: {DateTimeOffset.Now}",
                    CreateTime = DateTime.Now,
                    LogType = EnumLogTypes.Information
                };

                await logsService.Create(log);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
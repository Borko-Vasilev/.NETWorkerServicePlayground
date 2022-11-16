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

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("The service has been stoped...");

            LogModel log = new()
            {
                Message = $"The service has been stoped at: {DateTime.Now}",
                CreateTime = DateTime.Now,
                LogType = EnumLogTypes.Warning
            };

            await logsService.Create(log);

            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTime.Now);

                LogModel log = new()
                {
                    Message = $"Worker running at: {DateTime.Now}",
                    CreateTime = DateTime.Now,
                    LogType = EnumLogTypes.Information
                };

                await logsService.Create(log);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
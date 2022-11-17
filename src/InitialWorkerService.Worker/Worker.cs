using Hangfire;
using InitialWorkerService.Contracts.Logs;
using InitialWorkerService.Contracts.Logs.Models;
using InitialWorkerService.Services;

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

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await LoggingMessage($"The service has been started at: {DateTime.Now}", EnumLogTypes.Information, _logger);

            RecurringJob.AddOrUpdate<IWorkService>("message-logging", item => item.DoWork(), Cron.Minutely);

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await LoggingMessage($"The service has been stoped at: {DateTime.Now}", EnumLogTypes.Warning, _logger);
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1, stoppingToken);
            }
        }

        private async Task<LogModel> LoggingMessage(string message, EnumLogTypes logType, ILogger<Worker> logger)
        {
            switch (logType)
            {
                case EnumLogTypes.Success:
                    logger.LogInformation(message);
                    break;
                case EnumLogTypes.Error:
                    logger.LogError(message);
                    break;
                case EnumLogTypes.Warning:
                    logger.LogWarning(message);
                    break;
                case EnumLogTypes.Information:
                    logger.LogInformation(message);
                    break;
                case EnumLogTypes.Fatal:
                    logger.LogCritical(message);
                    break;
                default:
                    logger.LogInformation(message);
                    break;
            }

            LogModel log = new()
            {
                Message = message,
                CreateTime = DateTime.Now,
                LogType = logType
            };

            return await logsService.Create(log);
        }
    }
}
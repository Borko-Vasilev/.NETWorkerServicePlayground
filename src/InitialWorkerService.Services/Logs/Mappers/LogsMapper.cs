using InitialWorkerService.Contracts.Logs.Models;
using InitialWorkerService.Domain.Logs.Models;

namespace InitialWorkerService.Services.Logs.Mappers
{
    internal static class LogsMapper
    {
        public static Log Map(LogModel log)
        {
            Log mappedLog = new()
            {
                Message = log.Message,
                CreateTime = log.CreateTime,
                LogType = (Domain.Logs.Models.EnumLogTypes)log.LogType,
            };

            return mappedLog;
        }

        public static LogModel Map(Log log)
        {
            LogModel mappedLog = new()
            {
                Message = log.Message,
                CreateTime = log.CreateTime,
                LogType = (Contracts.Logs.Models.EnumLogTypes)log.LogType,
            };

            return mappedLog;
        }
    }
}

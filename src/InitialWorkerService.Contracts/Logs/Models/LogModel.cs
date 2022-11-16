namespace InitialWorkerService.Contracts.Logs.Models
{
    public class LogModel
    {
        public string Message { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Today;

        public EnumLogTypes LogType { get; set; }
    }
}

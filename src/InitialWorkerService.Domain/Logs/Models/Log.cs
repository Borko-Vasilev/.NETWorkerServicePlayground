namespace InitialWorkerService.Domain.Logs.Models
{
    public class Log
    {
        public int Id { get; set; }

        public string Message{ get; set; }

        public DateTime CreateTime { get; set; }

        public EnumLogTypes LogType { get; set; }
    }
}

namespace LoggingApi.Database
{
    public class LogModel
    {
        public int Id { get; set; }
        public string Request { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

}

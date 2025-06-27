namespace OnlineMongoMigrationProcessor.Models;

#pragma warning disable CS8618
public class LogObject
{
    public LogObject(LogType type, string message)
    {
        Message = message;
        Type = type;
        Datetime = DateTime.UtcNow;
    }

    public string Message { get; set; }
    public LogType Type { get; set; }
    public DateTime Datetime { get; set; }
}
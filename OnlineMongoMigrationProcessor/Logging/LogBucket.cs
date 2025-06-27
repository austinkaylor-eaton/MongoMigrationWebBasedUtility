using OnlineMongoMigrationProcessor.Models;

namespace OnlineMongoMigrationProcessor.Logging;

#pragma warning disable CS8602
public class LogBucket
{
    public List<LogObject>? Logs { get; set; }
    private List<LogObject>? _verboseMessages;
    private readonly object _lock = new object();

    public void AddVerboseMessage(string message, LogType logType = LogType.Message)
    {
        lock (_lock)
        {
            _verboseMessages ??= new List<LogObject>();

            if (_verboseMessages.Count == 5)
            {
                _verboseMessages.RemoveAt(0); // Remove the oldest item
            }
            _verboseMessages.Add(new LogObject(logType, message)); // Add the new item
        }
    }

    public List<LogObject> GetVerboseMessages()
    {
        try
        {
            _verboseMessages ??= new List<LogObject>();
            var reversedList = new List<LogObject>(_verboseMessages); // Create a copy to avoid modifying the original list
            reversedList.Reverse(); // Reverse the copy

            // If the reversed list has fewer than 5 elements, add empty message LogObjects
            while (reversedList.Count < 5)
            {
                reversedList.Add(new LogObject(LogType.Message, ""));
            }
            return reversedList;
        }
        catch
        {
            var blankList = new List<LogObject>();
            for (int i = 0; i < 5; i++)
            {
                blankList.Add(new LogObject(LogType.Message, ""));
            }
            return blankList;
        }
    }
}
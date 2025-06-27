using Newtonsoft.Json;

namespace OnlineMongoMigrationProcessor.Models;

#pragma warning disable CS8618
public class JobList
{
    public List<MigrationJob>? MigrationJobs { get; set; }
    public int ActiveRestoreProcessId { get; set; } = 0;
    public int ActiveDumpProcessId { get; set; } = 0;
    private string _filePath = string.Empty;
    private static readonly object _fileLock = new object();

    public JobList()
    {
        if (!Directory.Exists($"{Helper.GetWorkingFolder()}migrationjobs"))
        {
            Directory.CreateDirectory($"{Helper.GetWorkingFolder()}migrationjobs");
        }
        _filePath = $"{Helper.GetWorkingFolder()}migrationjobs\\list.json";
    }

    public void Load()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                var loadedObject = JsonConvert.DeserializeObject<JobList>(json);
                if (loadedObject != null)
                {
                    MigrationJobs = loadedObject.MigrationJobs;
                }
            }
        }
        catch (Exception ex)
        {
            Log.WriteLine($"Error loading data: {ex.ToString()}");
        }
    }

    public bool Save()
    {
        try
        {
            lock (_fileLock)
            {
                string json = JsonConvert.SerializeObject(this);
                //File.WriteAllText(_filePath, json);
                string tempFile = _filePath + ".tmp";
                File.WriteAllText(tempFile, json);
                File.Move(tempFile, _filePath, true); // Atomic move on most OSes

            }
            return true;
        }
        catch (Exception ex)
        {
            Log.WriteLine($"Error saving data: {ex.ToString()}", LogType.Error);
            return false;
        }
    }
}
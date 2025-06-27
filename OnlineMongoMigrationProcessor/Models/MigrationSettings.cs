using Newtonsoft.Json;

namespace OnlineMongoMigrationProcessor.Models;

#pragma warning disable CS8618
public class MigrationSettings
{
    public string? MongoToolsDownloadUrl { get; set; }
    public bool HasUuid { get; set; }
    public long ChunkSizeInMb { get; set; }
    public int ChangeStreamBatchSize { get; set; }
    public int MongoCopyPageSize { get; set; }
    private string _filePath = string.Empty;

    public MigrationSettings()
    {
        _filePath = $"{Helper.GetWorkingFolder()}migrationjobs\\config.json";
    }

    public void Load()
    {
        bool initialized = false;
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            var loadedObject = JsonConvert.DeserializeObject<MigrationSettings>(json);
            if (loadedObject != null)
            {
                HasUuid = loadedObject.HasUuid;
                MongoToolsDownloadUrl = loadedObject.MongoToolsDownloadUrl;
                ChunkSizeInMb = loadedObject.ChunkSizeInMb;
                ChangeStreamBatchSize = loadedObject.ChangeStreamBatchSize;
                MongoCopyPageSize=loadedObject.MongoCopyPageSize;
                initialized = true;
            }
        }
        if (!initialized)
        {
            HasUuid = false;
            MongoToolsDownloadUrl = "https://fastdl.mongodb.org/tools/db/mongodb-database-tools-windows-x86_64-100.10.0.zip";
            ChunkSizeInMb = 5120;
            ChangeStreamBatchSize = 10000;
            MongoCopyPageSize = 500;
        }
    }

    public bool Save()
    {
        try
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(_filePath, json);
            return true;
        }
        catch (Exception ex)
        {
            Log.WriteLine($"Error saving data: {ex.ToString()}", LogType.Error);
            return false;
        }
    }
}
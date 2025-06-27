using Newtonsoft.Json;

namespace OnlineMongoMigrationProcessor.Models;

#pragma warning disable CS8618
public class MigrationJob
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? SourceEndpoint { get; set; }
    public string? TargetEndpoint { get; set; }
    [JsonIgnore]
    public string? SourceConnectionString { get; set; }
    [JsonIgnore]
    public string? TargetConnectionString { get; set; }
    public string? SourceServerVersion { get; set; }
    public string? NameSpaces { get; set; }
    public DateTime? StartedOn { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsOnline { get; set; }
    public bool IsCancelled { get; set; }
    public bool IsStarted { get; set; }
    public bool CurrentlyActive { get; set; }
    public bool UseMongoDump { get; set; }
    public bool IsSimulatedRun { get; set; }
    public bool SkipIndexes { get; set; }
    public bool AppendMode { get; set; }
    public List<MigrationUnit>? MigrationUnits { get; set; }
}
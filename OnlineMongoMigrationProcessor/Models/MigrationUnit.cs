using MongoDB.Bson;
using MongoDB.Driver;

namespace OnlineMongoMigrationProcessor.Models;

#pragma warning disable CS8618
public class MigrationUnit
{
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
    public string? ResumeToken { get; set; }
    public ChangeStreamOperationType ResumeTokenOperation { get; set; }
    public BsonValue? ResumeDocumentId { get; set; }
    public DateTime? ChangeStreamStartedOn { get; set; }
    public DateTime CursorUtcTimestamp { get; set; }
    public double DumpPercent { get; set; }
    public double RestorePercent { get; set; }
    public bool DumpComplete { get; set; }
    public bool RestoreComplete { get; set; }
    public long EstimatedDocCount { get; set; }
    public CollectionStatus SourceStatus { get; set; }
    public long ActualDocCount { get; set; }
    public long DumpGap { get; set; }
    public long RestoreGap { get; set; }
    public List<MigrationChunk> MigrationChunks { get; set; }

    public MigrationUnit(string databaseName, string collectionName, List<MigrationChunk> migrationChunks)
    {
        DatabaseName = databaseName;
        CollectionName = collectionName;
        MigrationChunks = migrationChunks;
    }
}
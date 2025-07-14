using MongoDB.Bson;
using MongoDB.Driver;

namespace OnlineMongoMigrationProcessor.Models;

/// <summary>
/// A MigrationUnit represents a single unit of migration for a specific database and collection.
/// </summary>
/// <remarks>
/// 1. Can a Migration Unit be used for multiple collections? No, it is designed to represent a single collection migration. <br/>
/// 2. Can a Migration Unit be used for multiple databases? No, it is designed to represent a single database migration. <br/>
/// </remarks>
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
    /// <summary>
    /// Represents the gap in the number of documents between the dump and restore operations.
    /// </summary>
    public long DumpGap { get; set; }

    /// <summary>
    /// Represents the gap in the number of documents that need to be restored after the initial dump.
    /// </summary>
    public long RestoreGap { get; set; }

    /// <summary>
    /// A list of migration chunks that represent the data to be migrated for this migration unit.
    /// </summary>
    /// <remarks>Migration Chunks make it easier to process a Migration Unit by breaking the Migration Unit into "chunks"</remarks>
    public List<MigrationChunk> MigrationChunks { get; set; }

    public MigrationUnit(string databaseName, string collectionName, List<MigrationChunk> migrationChunks)
    {
        DatabaseName = databaseName;
        CollectionName = collectionName;
        MigrationChunks = migrationChunks;
    }
}
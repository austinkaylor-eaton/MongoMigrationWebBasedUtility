namespace OnlineMongoMigrationProcessor.Models;

#pragma warning disable CS8618
public class MigrationChunk
{
    /// <summary>
    /// The end ID of the chunk, exclusive.
    /// </summary>
    public string? Lt { get; set; }

    /// <summary>
    /// The start ID of the chunk, inclusive.
    /// </summary>
    public string? Gte { get; set; }
    public bool? IsDownloaded { get; set; }
    public bool? IsUploaded { get; set; }
    public long DumpQueryDocCount { get; set; }
    public long DumpResultDocCount { get; set; }
    public long RestoredSuccessDocCount { get; set; }
    public long RestoredFailedDocCount { get; set; }
    public long DocCountInTarget { get; set; }
    public long SkippedAsDuplicateCount { get; set; }
    public DataType DataType { get; set; }
    public List<Segment> Segments { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public MigrationChunk(string startId, string endId, DataType dataType, bool? downloaded, bool? uploaded)
    {
        Gte = startId;
        Lt = endId;
        IsDownloaded = downloaded;
        IsUploaded = uploaded;
        DataType = dataType;
    }
}
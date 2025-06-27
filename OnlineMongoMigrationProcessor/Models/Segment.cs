namespace OnlineMongoMigrationProcessor.Models;

#pragma warning disable CS8618
public class Segment
{
    public string? Lt { get; set; }
    public string? Gte { get; set; }
    public bool? IsProcessed { get; set; }
    public long QueryDocCount { get; set; }
    public string Id { get; set; }
}
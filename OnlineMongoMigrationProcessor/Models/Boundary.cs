using MongoDB.Bson;

namespace OnlineMongoMigrationProcessor.Models;

#pragma warning disable CS8618
public class Boundary
{
    public BsonValue? StartId { get; set; }
    public BsonValue? EndId { get; set; }
    public List<Boundary> SegmentBoundaries { get; set; }
}
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace PlaygroundConsole;

public static class CosmosVCoreChangeStreamProcessor
{
    private const string ConnectionString = "<mongo_connection_string>";

    public static async Task RunAsync()
    {
        MongoClient client = new(ConnectionString);
        IMongoDatabase? database = client.GetDatabase("<database_name>");
        IMongoCollection<BsonDocument>? collection = database.GetCollection<BsonDocument>("<collection_name>");

        // Open a change stream
        using IChangeStreamCursor<ChangeStreamDocument<BsonDocument>>? cursor = collection.Watch();
        try
        {
            await Console.Out.WriteLineAsync($"Starting change stream for {client.Settings.Server.Host} on collection {collection.CollectionNamespace.FullName}");
            foreach (ChangeStreamDocument<BsonDocument>? change in cursor.ToEnumerable())
            {
                JObject json = new()
                {
                    ["operationType"] = change.OperationType.ToString(),
                    ["namespace"] = change.CollectionNamespace?.FullName
                };

                if (change.DocumentKey != null)
                {
                    json["documentKey"] = change.DocumentKey.ToJson();
                }

                if (change.ResumeToken != null)
                {
                    json["resumeToken"] = change.ResumeToken.ToJson();
                }

                if (change.FullDocument != null)
                {
                    json["fullDocument"] = change.FullDocument.ToJson();
                }

                if (change.WallTime != null)
                {
                    json["wallTime"] = change.WallTime.ToString();
                }

                Console.WriteLine(json.ToString(Newtonsoft.Json.Formatting.Indented));
            }
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync("Error processing change stream: " + e);
        }
    }

    /// <summary>
    /// Runs a change stream with a resume token.
    /// </summary>
    public static async Task RunWithResumeTokenAsync()
    {
        MongoClient client = new(ConnectionString);
        IMongoDatabase database = client.GetDatabase("<database_name>");
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("<collection_name>");

        BsonDocument? resumeToken = LoadResumeToken();

        ChangeStreamOptions options = new();
        if (resumeToken != null)
        {
            options.ResumeAfter = resumeToken;
        }

        using IChangeStreamCursor<ChangeStreamDocument<BsonDocument>> cursor = collection.Watch(options);

        try
        {
            await Console.Out.WriteLineAsync($"Starting change stream for {client.Settings.Server.Host} on collection {collection.CollectionNamespace.FullName}");
            foreach (ChangeStreamDocument<BsonDocument> change in cursor.ToEnumerable())
            {
                JObject json = new()
                {
                    ["operationType"] = change.OperationType.ToString(),
                    ["namespace"] = change.CollectionNamespace?.FullName
                };

                if (change.DocumentKey != null)
                    json["documentKey"] = change.DocumentKey.ToJson();

                if (change.ResumeToken != null)
                {
                    json["resumeToken"] = change.ResumeToken.ToJson();
                    SaveResumeToken(change.ResumeToken); // Save token after each change
                }

                if (change.FullDocument != null)
                    json["fullDocument"] = change.FullDocument.ToJson();

                if (change.WallTime != null)
                    json["wallTime"] = change.WallTime.ToString();

                await Console.Out.WriteLineAsync("Change detected:");
                await Console.Out.WriteLineAsync(json.ToString(Newtonsoft.Json.Formatting.Indented));
            }
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync("Error processing change stream: " + e);
        }
    }

    /// <summary>
    /// Saves a resume token to a file.
    /// </summary>
    /// <param name="token">The resume token as a <see cref="BsonDocument"/></param>
    private static void SaveResumeToken(BsonDocument token)
    {
        Console.WriteLine("Saving resume token...");
        const string tokenFilePath = "resumeToken.json";
        File.WriteAllText(tokenFilePath, token.ToJson());
    }

    /// <summary>
    /// Loads the resume token from a file.
    /// </summary>
    /// <returns>
    /// The resume token as a <see cref="BsonDocument"/>, or null if the file does not exist.
    /// </returns>
    private static BsonDocument? LoadResumeToken()
    {
        Console.WriteLine("Loading resume token...");
        const string tokenFilePath = "resumeToken.json";
        if (!File.Exists(tokenFilePath)) return null;
        // Read the resume token from the file
        string json = File.ReadAllText(tokenFilePath);
        return BsonDocument.Parse(json);
    }


}
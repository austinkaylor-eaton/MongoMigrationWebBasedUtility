using OnlineMongoMigrationProcessor.Models;

namespace OnlineMongoMigrationProcessor.Interface
{
    public interface IMigrationProcessor
    {
        // Properties
        bool ProcessRunning { get; set; }

        // Methods
        void StopProcessing();
        void Migrate(MigrationUnit item, string sourceConnectionString, string targetConnectionString, string idField = "_id");        
        //void Upload(MigrationUnit item, string targetConnectionString);
    }
}

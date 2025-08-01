﻿using System;
using System.Collections.Generic;
using OnlineMongoMigrationProcessor;

namespace MongoMigrationWebApp.Service
{
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8604

    public class JobManager
    {
        private JobList? _jobList;
        private MigrationWorker? MigrationWorker { get; set; }

        public JobManager()
        {
            
        }

        

        #region _configuration Management

        public bool UpdateConfig(OnlineMongoMigrationProcessor.MigrationSettings updated_config,out string errorMessage)
        {           
            if (updated_config == null)
            {
                errorMessage = "Migration settings cannot be null.";
                return false;
            }
            // Save the updated config
            return updated_config.Save(out errorMessage);
        }

        public OnlineMongoMigrationProcessor.MigrationSettings GetConfig()
        {
            MigrationSettings config = new MigrationSettings();
            config.Load();
            return config;
        }

        #endregion 
        #region Job Management


        public DateTime GetJobBackupDate()
        {
            return _jobList.GetBackupDate();
        }


        public bool RestoreJobsFromBackup(out string errorMessage)
        {
            _jobList = null;
            _jobList = new JobList();

            var success = _jobList.LoadJobs(out errorMessage,true);
            if (!success)
            {
                return false;
            }

            if (MigrationWorker != null)
            {
                MigrationWorker.StopMigration();
                MigrationWorker = null;         
            }

            MigrationWorker =new MigrationWorker(_jobList);

            errorMessage = string.Empty;
            return success;
        }

        public bool SaveJobs(out string errorMessage)
        {
            return _jobList.Save(out errorMessage);
        }

        public List<MigrationJob> GetMigrations(out string errorMessage)
        {
            errorMessage=string.Empty;
            if (_jobList == null)
            {
                _jobList = new JobList();
                _jobList.LoadJobs(out errorMessage, false);
            }

            if (_jobList.MigrationJobs == null)
            {
                _jobList.MigrationJobs = new List<MigrationJob>();
                SaveJobs(out errorMessage);
            }

            return _jobList.MigrationJobs ??= new List<MigrationJob>();
        }

        public void ClearJobFiles(string jobId)
        {
            try
            {
                System.IO.Directory.Delete($"{Helper.GetWorkingFolder()}mongodump\\{jobId}", true);
            }
            catch
            {
            }
        }

        #endregion 
        #region Log Management

        public List<LogObject> GetVerboseMessages(string id)
        {
            //verbose messages  are only  there for active jobList so fetech from migration worker.
            if (MigrationWorker != null && MigrationWorker.IsProcessRunning(id))
               return MigrationWorker.GetVerboseMessages(id);
            else
               return new List<LogObject>();
        }

        public LogBucket GetLogBucket(string id,out string fileName, out bool isLiveLog)
        {
            //Check if migration workewr is initialized and active. Return migration workers log bucket if it is.
            LogBucket? bucket = null;
            if (MigrationWorker != null && MigrationWorker.IsProcessRunning(id)) //only if worker's current job Id matches param
            {
                //Console.WriteLine($"Migration worker is running for job ID: {id}");
                bucket = MigrationWorker.GetLogBucket(id); 
                isLiveLog = true;
                fileName=string.Empty;
                return bucket;
            }

            //If migration worker is not running, get the log bucket from the file.Its static  
            isLiveLog=false;
            Log log = new Log();
            return log.ReadLogFile(id,out fileName);
        }

        #endregion

        #region Migration Worker Management

        public void StopMigration()
        {
            MigrationWorker?.StopMigration();
        }

        public Task CancelMigration(string id)
        {
            var migration = _jobList.MigrationJobs.Find(m => m.Id == id);
            if (migration != null)
            {
                migration.IsCancelled = true;
                migration.IsStarted = false;
            }
            return Task.CompletedTask;
        }

        public async Task StartMigrationAsync(MigrationJob job, string sourceConnectionString, string targetConnectionString, string namespacesToMigrate, bool doBulkCopy, bool trackChangeStreams)
        {

            MigrationWorker = new MigrationWorker(_jobList);

            MigrationWorker?.StartMigrationAsync(job, sourceConnectionString, targetConnectionString, namespacesToMigrate, doBulkCopy, trackChangeStreams);
        }


        public void SyncBackToSource(string sourceConnectionString, string targetConnectionString, MigrationJob job)
        {

            MigrationWorker = new MigrationWorker(_jobList);
            MigrationWorker?.SyncBackToSource(sourceConnectionString, targetConnectionString, job);
        }


        public string GetRunningJobId()
        {
            return MigrationWorker?.GetRunningJobId() ?? string.Empty;
        }

        public bool IsProcessRunning(string id)
        {
            return MigrationWorker?.IsProcessRunning(id) ?? false;
        }

        #endregion

    }
}


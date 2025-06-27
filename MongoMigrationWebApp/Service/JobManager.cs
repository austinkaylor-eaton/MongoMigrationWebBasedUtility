﻿using OnlineMongoMigrationProcessor;
using OnlineMongoMigrationProcessor.Helpers;
using OnlineMongoMigrationProcessor.Logging;
using OnlineMongoMigrationProcessor.Models;

namespace MongoMigrationWebApp.Service;
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8604

public class JobManager
{
    private readonly JobList? _jobList;
    public MigrationWorker? MigrationWorker { get; set; }
    private List<LogObject>? LogBucket { get; set; }

    public JobManager()
    {
        if (_jobList == null)
        {
            _jobList = new JobList();
            _jobList.Load();
        }

        if (MigrationWorker == null)
        {
            MigrationWorker = new MigrationWorker(_jobList);
        }

        if (_jobList.MigrationJobs == null)
        {
            _jobList.MigrationJobs = new List<MigrationJob>();
            Save();
        }
    }

    public bool Save()
    {
        return _jobList.Save();
    }

    public List<MigrationJob> GetMigrations() => _jobList.MigrationJobs;

    public LogBucket GetLogBucket(string id) => Log.GetLogBucket(id);

    public void DisposeLogs()
    {
        Log.Dispose();
    }

    public Task CancelMigration(string id)
    {
        var migration = _jobList.MigrationJobs.Find(m => m.Id == id);
        if (migration != null)
        {
            migration.IsCancelled = true;
        }
        return Task.CompletedTask;
    }

    public Task ResumeMigration(string id)
    {
        var migration = _jobList.MigrationJobs.Find(m => m.Id == id);
        if (migration != null)
        {
            migration.IsCancelled = true;
        }
        return Task.CompletedTask;
    }

    public Task ViewMigration(string id)
    {
        var migration = _jobList.MigrationJobs.Find(m => m.Id == id);
        if (migration != null)
        {
            migration.IsCancelled = true;
        }
        return Task.CompletedTask;
    }

    public void ClearJobFiles(string jobId)
    {
        try
        {
            Directory.Delete($"{Helper.GetWorkingFolder()}mongodump\\{jobId}",true);
        }
        catch
        {
        }
    }


}
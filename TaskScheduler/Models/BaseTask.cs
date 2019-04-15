using Microsoft.WindowsAzure.Storage.Table;
using NCrontab;
using System;
using static TaskScheduler.StorageNames;

namespace TaskScheduler.Models
{
    public class BaseTask : TableEntity
    {
        public BaseTask() : base(PARTITIONKEY_DEFAULT, Guid.NewGuid().ToString())
        {
        }

        public string Cron { get; set; }

        public DateTime NextOccurance { get; set; } = DateTime.Now;

        public string TaskType { get; set; }

        public void SetNextOccurance()
        {
            NextOccurance = CrontabSchedule.Parse(Cron).GetNextOccurrence(DateTime.Now);
        }
    }
}

using Microsoft.WindowsAzure.Storage.Table;
using NCrontab;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models
{
    public class BaseTask : TableEntity
    {
        public BaseTask() : base("TaskPartitionKey", Guid.NewGuid().ToString())
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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace Api.Models
{
    class TaskTableEntity : TableEntity
    {
        public TaskTableEntity()
        {
            PartitionKey = "TaskPartitionKey";
        }

        public string Cron { get; set; }

        public TaskType TaskType { get; set; }

        public string TaskOptions { get; set; }
    }
}

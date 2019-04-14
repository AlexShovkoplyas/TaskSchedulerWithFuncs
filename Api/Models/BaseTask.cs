using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models
{
    public class BaseTask : TableEntity
    {
        public BaseTask(): base("TaskPartitionKey", Guid.NewGuid().ToString())
        {

        }

        public string Cron { get; set; }

        public string TaskType { get; set; }
    }
}

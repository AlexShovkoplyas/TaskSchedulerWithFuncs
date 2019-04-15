using Microsoft.WindowsAzure.Storage.Table;
using System;
using static TaskScheduler.StorageNames;

namespace TaskScheduler.Models
{
    public class WebPingLog : TableEntity
    {
        public WebPingLog(string rowKey) : base(PARTITIONKEY_DEFAULT, rowKey)
        {
        }

        public DateTime Processed { get; set; } = DateTime.UtcNow;

        public bool IsSucsess { get; set; }
    }
}

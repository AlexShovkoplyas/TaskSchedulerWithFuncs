using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Api.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Azure.ServiceBus;
using System.Text;

namespace Api
{
    public static class Scheduler
    {

        [FunctionName("Schedule_WebPing")]
        public static async Task Run(
            [QueueTrigger("scheduled-tasks", Connection = "TableStorage")] string myQueueItem,
            [Table("Tasks2", "TaskPartitionKey", "{queueTrigger}", Connection = "TableStorage")] WebPingTask task,
            [ServiceBus("WebPing", Connection = "ServiceBusConnection")] IAsyncCollector<Message> serviceBus,
            ILogger log)
        {
            task.SetNextOccurance();

            var message = new Message();
            message.MessageId = task.RowKey;
            message.UserProperties.Add("RowKey", task.RowKey);
            message.ScheduledEnqueueTimeUtc = task.NextOccurance.ToUniversalTime();

            await serviceBus.AddAsync(message);
        }
    }
}

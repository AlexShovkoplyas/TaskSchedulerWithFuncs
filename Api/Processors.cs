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
    public static class Function3
    {
        
        [FunctionName("Process_WebPing")]
        [return: Table("WebPingLog", Connection = "TableStorage")]
        public static async Task<WebPingLog> Run(
            [ServiceBusTrigger("WebPing", Connection = "ServiceBusConnection")] Message taskMessage,
            [ServiceBus("WebPing", Connection = "ServiceBusConnection")] IAsyncCollector<Message> serviceBus,
            [Table("Tasks2", Connection = "TableStorage")] CloudTable logTable,
            ILogger log)
        {
            log.LogInformation("PROCESSOR :");

            var taskJson = Encoding.UTF8.GetString(taskMessage.Body);

            var taskObj = JsonConvert.DeserializeObject<WebPingTask>(taskJson);
            log.LogInformation(taskJson);

            var operation = TableOperation.Retrieve(taskObj.PartitionKey, taskObj.RowKey);
            var task = (WebPingTask)(await logTable.ExecuteAsync(operation)).Result;
            task.SetNextOccurance();

            var operationAdd = TableOperation.InsertOrMerge(task);
            await logTable.ExecuteAsync(operationAdd);

            return new WebPingLog { IsSucsess = true };
        }
    }
}

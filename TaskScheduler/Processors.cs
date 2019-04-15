using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TaskScheduler.Models;
using Microsoft.Azure.ServiceBus;
using static TaskScheduler.StorageNames;

namespace TaskScheduler
{
    public static class Processors
    {        
        [FunctionName("ProcessTaskWebPing")]
        [return: Table(LOG_WEBPING_TABLE_NAME)]
        public static async Task<WebPingLog> Run(
            [ServiceBusTrigger("WebPing")] Message taskMessage,
            [Queue("scheduled-tasks")] IAsyncCollector<string> queue,
            [Table(TASK_WEBPING_TABLE_NAME, PARTITIONKEY_DEFAULT, "{Label}")] WebPingTask webPingTask,
            ILogger log)
        {
            await queue.AddAsync(webPingTask.RowKey);

            // Process Web Ping -- webPingTask -- ...

            return new WebPingLog(webPingTask.RowKey) { IsSucsess = true };
        }
    }
}

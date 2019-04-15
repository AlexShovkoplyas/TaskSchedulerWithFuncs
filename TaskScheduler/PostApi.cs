using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TaskScheduler.Models;
using Microsoft.WindowsAzure.Storage.Table;
using static TaskScheduler.StorageNames;

namespace TaskScheduler
{
    public static class PostApi
    {
        [FunctionName("CreateTaskWebPing")]
        [return: Queue(SCHEDULE_WEBPING_QUEUE_NAME)]
        public static async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Tasks/WebPing")] HttpRequest req,
            [Table(TASK_WEBPING_TABLE_NAME)] CloudTable taskTable,            
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var task = JsonConvert.DeserializeObject<WebPingTask>(requestBody);
            task.SetNextOccurance();

            var operation = TableOperation.Insert(task);
            await taskTable.ExecuteAsync(operation);

            return task.RowKey;
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TaskScheduler.Models;
using Microsoft.WindowsAzure.Storage.Table;
using static TaskScheduler.StorageNames;

namespace TaskScheduler
{
    public static class GetApi
    {
        [FunctionName("GetTasksWebPing")]
        public async static Task<IActionResult> Run3(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Tasks/WebPing")] HttpRequest req,
            [Table(TASK_WEBPING_TABLE_NAME)] CloudTable taskTable,
            ILogger log)
        {
            var segment = await GetTasks<WebPingTask>(taskTable, "WebPing");

            return new OkObjectResult(segment);
        }

        private async static Task<TableQuerySegment<T>> GetTasks<T>(CloudTable taskTable, string taskType) where T : BaseTask, new()
        {
            var query = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition("TaskType", QueryComparisons.Equal, taskType));
            var segment = await taskTable.ExecuteQuerySegmentedAsync(query, null);

            return segment;
        }
    }
}

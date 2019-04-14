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

namespace Api
{
    public static class Function2
    {
        [FunctionName("CreateTask_WebPing")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Tasks/WebPing")] HttpRequest req,
            [Table("Tasks2", Connection = "TableStorage")] CloudTable taskTable,
            [CosmosDB("Tasks", "Items",
                ConnectionStringSetting = "CosmosDBConnection",
                SqlQuery = "select * from Tasks order by")]
                IEnumerable<ToDoItem> toDoItems,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var task = JsonConvert.DeserializeObject<WebPingTask>(requestBody);

            var operation = TableOperation.Insert(task);
            var c = await taskTable.ExecuteAsync(operation);

            return new OkObjectResult("Added.");
        }

        [FunctionName("GetAllTasks")]
        public async static Task<IActionResult> Run25(
            [TimerTrigger("0 */1 * * * *", RunOnStartup = true)] TimerInfo timerInfo,
            [Table("Tasks2", Connection = "TableStorage")] CloudTable taskTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var segment1 = await GetTasks<WebPingTask>(taskTable, "WebPing");
            var segment2 = await GetTasks<SavePageTask>(taskTable, "SavePage");

            return new OkObjectResult(segment1.Cast<BaseTask>().Concat(segment2).Cast<BaseTask>());
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

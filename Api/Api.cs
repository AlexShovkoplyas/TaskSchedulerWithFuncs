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

namespace Api
{
    public static class Function1
    {
        [FunctionName("CreateTask_WebPing")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Tasks/WebPing")] HttpRequest req,
            [Table("Tasks2", Connection = "TableStorage")] CloudTable taskTable,
            [CosmosDB(
                databaseName: "ToDoItems",
                collectionName: "Items",
                ConnectionStringSetting = "CosmosDBConnection")] dynamic taskScheduled,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var task = JsonConvert.DeserializeObject<WebPingTask>(requestBody);

            var operation = TableOperation.Insert(task);
            var c = await taskTable.ExecuteAsync(operation);

            //TODO:
            taskScheduled = new { Description = queueMessage, id = Guid.NewGuid() };

            return new OkObjectResult("Added.");
        }

        [FunctionName("CreateTask_PageSave")]
        public static async Task<IActionResult> Run2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Tasks/PageSave")] HttpRequest req,
            [Table("Tasks2", Connection = "TableStorage")] CloudTable taskTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var task = JsonConvert.DeserializeObject<SavePageTask>(requestBody);

            var operation = TableOperation.Insert(task);
            var c = await taskTable.ExecuteAsync(operation);

            return new OkObjectResult("Added.");
        }

        [FunctionName("GetAllTasks")]
        public async static Task<IActionResult> Run25(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Tasks")] HttpRequest req,
            [Table("Tasks2", Connection = "TableStorage")] CloudTable taskTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var segment1 = await GetTasks<WebPingTask>(taskTable, "WebPing");
            var segment2 = await GetTasks<SavePageTask>(taskTable, "SavePage");

            return new OkObjectResult(segment1.Cast<BaseTask>().Concat(segment2).Cast<BaseTask>());
        }

        [FunctionName("GetAllTasks_WebPing")]
        public async static Task<IActionResult> Run3(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Tasks/WebPing")] HttpRequest req,
            [Table("Tasks2", Connection = "TableStorage")] CloudTable taskTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var segment = await GetTasks<WebPingTask>(taskTable, "WebPing");

            return new OkObjectResult(segment);
        }

        [FunctionName("GetAllTasks_SavePage")]
        public async static Task<IActionResult> Run4(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Tasks/SavePage")] HttpRequest req,
            [Table("Tasks2", Connection = "TableStorage")] CloudTable taskTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var segment = await GetTasks<SavePageTask>(taskTable, "SavePage");

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

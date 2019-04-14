//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Api.Models
//{
//    public static class MapperExtension
//    {
//        public static TaskTableEntity ToEntity(this BaseTask task) =>
//            new TaskTableEntity
//            {
//                RowKey = task.Id,
//                PartitionKey = "PartitionKey1",
//                Cron = task.Cron,
//                TaskType = task.TaskType,
//                //TaskOptionsJson = task.TaskOptionsJson
//            };

//        public static TaskTableEntity ToEntity(this WebPingTask task) =>
//            new TaskTableEntity
//            {
//                RowKey = task.Id,
//                PartitionKey = "PartitionKey1",
//                Cron = task.Cron,
//                TaskType = task.TaskType,
//                //TaskOptionsJson = task.TaskOptionsJson
//            };

//        public static TaskTableEntity ToEntity(this BaseTask task) =>
//            new TaskTableEntity
//            {
//                RowKey = task.Id,
//                PartitionKey = "PartitionKey1",
//                Cron = task.Cron,
//                TaskType = task.TaskType,
//                //TaskOptionsJson = task.TaskOptionsJson
//            };

//        public static BaseTask ToModel(this TaskTableEntity task)
//        {
//            switch (task.TaskType)
//            {
//                case TaskType.WebPing:
//                    var options = JsonConvert.DeserializeObject(task.TaskOptionsJson) as WebPingOptions;
//                    if (options is null)
//                    {
//                        throw new Exception("Can't deserialize");
//                    }
//                    return new WebPingTask
//                    {
//                        Id = task.RowKey,
//                        Cron = task.Cron,
//                        TaskType = task.TaskType,
//                        //TaskOptions = options
//                    };

//                default:
//                    throw new Exception("Can't read data from storage");
//            }
//        }
//    }
//}

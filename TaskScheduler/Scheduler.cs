using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TaskScheduler.Models;
using Microsoft.Azure.ServiceBus;
using static TaskScheduler.StorageNames;

namespace TaskScheduler
{
    public static class Scheduler
    {
        [FunctionName("ScheduleTaskWebPing")]
        [return: ServiceBus(PROCESS_WEBPING_BUSQUEUE_NAME)]
        public static async Task<Message> Run(
            [QueueTrigger(SCHEDULE_WEBPING_QUEUE_NAME)] string myQueueItem,
            [Table(TASK_WEBPING_TABLE_NAME, PARTITIONKEY_DEFAULT, "{queueTrigger}")] WebPingTask task,
            ILogger log)
        {
            task.SetNextOccurance();

            var message = new Message();
            message.Label = task.RowKey;
            message.ScheduledEnqueueTimeUtc = task.NextOccurance.ToUniversalTime();

            return message;
        }
    }
}

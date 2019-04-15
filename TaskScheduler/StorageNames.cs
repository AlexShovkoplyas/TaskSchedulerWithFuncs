namespace TaskScheduler
{
    public static class StorageNames
    {
        public const string TASK_WEBPING_TABLE_NAME = "TasksBase";
        public const string TASK_SAVEPAGE_TABLE_NAME = "TasksBase";

        public const string SCHEDULE_WEBPING_QUEUE_NAME = "scheduled-webping-tasks";
        public const string SCHEDULE_SAVEPAGE_QUEUE_NAME = "scheduled-webping-tasks";

        public const string PROCESS_WEBPING_BUSQUEUE_NAME = "WebPing";
        public const string PROCESS_SAVEPAGE_BUSQUEUE_NAME = "SavePage";

        public const string LOG_WEBPING_TABLE_NAME = "WebPingLog";
        public const string LOG_SAVEPAGE_TABLE_NAME = "SavePageLog";

        public const string PARTITIONKEY_DEFAULT = "partition";
    }
}

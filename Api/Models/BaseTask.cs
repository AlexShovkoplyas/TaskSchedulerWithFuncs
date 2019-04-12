using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models
{
    public class BaseTask
    {
        public string Id { get; set; }

        public string Cron { get; set; }

        public TaskType TaskType { get; set; }

        public object TaskOptions { get; set; }
    }
}

using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models
{
    public class WebPingLog : TableEntity
    {
        public WebPingLog() : base("TaskPartitionKey", Guid.NewGuid().ToString())
        {

        }

        public bool IsSucsess { get; set; }
    }
}

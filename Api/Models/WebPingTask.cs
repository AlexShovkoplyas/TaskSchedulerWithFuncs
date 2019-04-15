using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models
{
    public class WebPingTask : BaseTask
    {
        public string Url { get; set; }        
    }
}

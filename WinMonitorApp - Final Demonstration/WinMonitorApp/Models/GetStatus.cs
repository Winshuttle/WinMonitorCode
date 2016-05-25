using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WinMonitorApp.Models
{
    public class GetStatus
    {
        public string DataPrimary { get; set; }
        public string DataSecondary { get; set; }
        public string DataCenter { get; set; }
        public string ComponentName { get; set; }
        public string Status { get; set; }
    }
}
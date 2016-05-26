using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WinMonitorApp.Models
{
    public class DummyCalendar
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public System.DateTime Start { get; set; }
        public System.DateTime EndTime { get; set; }
        public string Repetition { get; set; }
        public string Maintenance { get; set; }
        public string Status { get; set; }
    }
}
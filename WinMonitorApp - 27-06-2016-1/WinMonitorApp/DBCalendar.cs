//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WinMonitorApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class DBCalendar
    {
        public int DBEventId { get; set; }
        public string DBEventTitle { get; set; }
        public string DBEventDetails { get; set; }
        public System.DateTime DBEventStartTime { get; set; }
        public System.DateTime DBEventEndTime { get; set; }
        public string DBEventDifferenceTime { get; set; }
        public string DBEventMaintenance { get; set; }
        public string DBEventStatus { get; set; }
        public Nullable<int> DBCompanyId { get; set; }
    
        public virtual DBCompany DBCompany { get; set; }
    }
}

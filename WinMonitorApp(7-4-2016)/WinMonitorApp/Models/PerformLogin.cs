using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WinMonitorApp.Models
{
    public class PerformLogin
    {

        // For Displaying List of Registered Administrators

        public List<DBLogin> mRetrieveLoginDetails()
        {
            WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
            return mDBContext.DBLogins.ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WinMonitorApp.Models
{
    public class PerformUserPerformance
    {
        public List<DummyPerformance> gettingEventTimeStored(int pstrComponentId, int eventStartOffset, int eventEndOffset, string pstrRecentOrNot)
        {
            WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
            DBLogHistory lastHistoryObj = new DBLogHistory();
            DateTime nowTime = DateTime.Now;
            DateTime eventStart, eventEnd;
            if (pstrRecentOrNot == "true")
            {
                eventStart = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day + eventStartOffset, nowTime.Hour, nowTime.Minute, nowTime.Second);
                eventEnd = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day + eventEndOffset, nowTime.Hour, nowTime.Minute, nowTime.Second);
            }
            else
            {
                eventStart = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day + eventStartOffset, 0, 0, 0);
                eventEnd = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day + eventEndOffset, 0, 0, 0);
            }

            List<DummyPerformance> listPerformance = new List<DummyPerformance>();
            DummyPerformance performanceObj;
            int a = 0;
            List<DBLogHistory> listHistory = new List<DBLogHistory>();
            listHistory = (from historyObj in contextObj.DBLogHistories
                           where ((historyObj.DBCSId == pstrComponentId) && (eventStart <= historyObj.DBDateTimeStart) && (historyObj.DBDateTimeStart <= eventEnd)&&(historyObj.DBStatus!="Operational"))
                           select historyObj).ToList();
            if (listHistory.Count > 0)
            {
                performanceObj = new DummyPerformance();
                a = (contextObj.GetDateDiffInSec(eventStart, listHistory[0].DBDateTimeStart).First()).Value;
                performanceObj.mLogId = -1;
                performanceObj.mDiff = a;
                performanceObj.mStatus = "Operational";
                listPerformance.Add(performanceObj);
                int i;
                DateTime? lastInList = DateTime.Now;
                for (i = 0; i < listHistory.Count; i++)
                {
                    if (listHistory[i].DBDateTimeEnd != null)
                    {
                        performanceObj = new DummyPerformance();
                        a = (contextObj.GetDateDiffInSec(listHistory[i].DBDateTimeStart, listHistory[i].DBDateTimeEnd).First()).Value;
                        performanceObj.mLogId = listHistory[i].DBLogId;
                        performanceObj.mDiff = a;
                        performanceObj.mStatus = listHistory[i].DBStatus;
                        listPerformance.Add(performanceObj);

                        if (i != listHistory.Count - 1)
                        {
                            performanceObj = new DummyPerformance();
                            a = (contextObj.GetDateDiffInSec(listHistory[i].DBDateTimeEnd, listHistory[i + 1].DBDateTimeStart).First()).Value;
                            performanceObj.mLogId = -1;
                            performanceObj.mDiff = a;
                            performanceObj.mStatus = "Operational";
                            listPerformance.Add(performanceObj);
                        }
                        else 
                        {
                            lastInList = listHistory[i].DBDateTimeEnd;
                            performanceObj = new DummyPerformance();
                            a = (contextObj.GetDateDiffInSec(lastInList, eventEnd).First()).Value;
                            performanceObj.mLogId = -1;
                            performanceObj.mDiff = a;
                            performanceObj.mStatus = "Operational";
                            listPerformance.Add(performanceObj);
                        }
                
                        
                    }
                    else 
                    {
                        performanceObj = new DummyPerformance();
                        a = (contextObj.GetDateDiffInSec(listHistory[i].DBDateTimeStart, eventEnd).First()).Value;
                        performanceObj.mLogId = listHistory[i].DBLogId;
                        performanceObj.mDiff = a;
                        performanceObj.mStatus = listHistory[i].DBStatus;
                        listPerformance.Add(performanceObj);
                    }
                }
                
            }
            else
            {
                performanceObj = new DummyPerformance();
                a = (contextObj.GetDateDiffInSec(eventStart, eventEnd).First()).Value;
                performanceObj.mLogId = -1;
                performanceObj.mDiff = a;
                performanceObj.mStatus = "Operational";
                listPerformance.Add(performanceObj);
            }

            return listPerformance;
        }


        public DBLogHistory gettingDetailsOfIncident(int pstrLogId) 
        {
            WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
            DBLogHistory historyObj = new DBLogHistory();
            historyObj = (from history in contextObj.DBLogHistories
                          where history.DBLogId == pstrLogId
                          select history).First();
            return historyObj;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WinMonitorApp.Models
{
    public class PerformUserPerformance
    {
        //function to get all the event duration with events
        public List<DummyPerformance> gettingEventTimeStored(int pstrComponentId, int eventStartOffset, int eventEndOffset, string pstrRecentOrNot)
        {
            WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
            DBLogHistory lastHistoryObj = new DBLogHistory();
            DateTime nowTime = DateTime.UtcNow;
            DateTime eventStart, eventEnd;
            if (pstrRecentOrNot == "true")
            {
                if (((nowTime.Day + eventStartOffset) <= 0)||((nowTime.Day + eventEndOffset)<= 0))
                {
                    eventStart = nowTime.AddDays(eventStartOffset);
                    eventEnd = nowTime.AddDays(eventEndOffset);
                }
                else {
                    eventStart = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day + eventStartOffset, nowTime.Hour, nowTime.Minute, nowTime.Second);
                    eventEnd = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day + eventEndOffset, nowTime.Hour, nowTime.Minute, nowTime.Second);
                }
            }
            else
            {
                if (((nowTime.Day + eventStartOffset) <= 0) || ((nowTime.Day + eventEndOffset) <= 0))
                {
                    eventStart = nowTime.AddDays(eventStartOffset);
                    eventEnd = nowTime.AddDays(eventEndOffset);
                    eventStart = new DateTime(eventStart.Year, eventStart.Month, eventStart.Day, 0, 0, 0);
                    eventEnd = new DateTime(eventEnd.Year, eventEnd.Month, eventEnd.Day, 0, 0, 0);
                    
                }
                else
                {
                    eventStart = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day + eventStartOffset, 0, 0, 0);
                    eventEnd = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day + eventEndOffset, 0, 0, 0);
                }
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
                DateTime? lastInList = DateTime.UtcNow;
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


        //function to get details of incidents
        public DBLogHistory gettingDetailsOfIncident(int pstrLogId) 
        {
            WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
            DBLogHistory historyObj = new DBLogHistory();
            historyObj = (from history in contextObj.DBLogHistories
                          where history.DBLogId == pstrLogId
                          select history).First();
            return historyObj;
        }

        //function to get expected end date
        public DummyClassToGetExpectedEndTime gettingExpectedEndTime(int componentId)
        {
            DummyClassToGetExpectedEndTime dummyObj = new DummyClassToGetExpectedEndTime();
            WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
            DBIncident incidentObj = new DBIncident();
            DBLogHistory historyObj = new DBLogHistory();
            incidentObj = contextObj.Database.SqlQuery<DBIncident>("select * from DBIncidents where DBCSId='" + componentId + "';").First();
            historyObj = contextObj.Database.SqlQuery<DBLogHistory>("select * from DBLogHistory where DBIncidentId='" + incidentObj.DBIncidentId + "';").Last();
            if (incidentObj.DBExpectedDuration != null)
            {
                dummyObj.expectedEndpresentOrNot = true;
                dummyObj.expectedEndDate = contextObj.Database.SqlQuery<DateTime>("select DATEADD(hour, " + incidentObj.DBExpectedDuration + ", '"+ historyObj.DBDateTimeStart +"')").First();
            }
            else {
                dummyObj.expectedEndpresentOrNot = false;
                dummyObj.expectedEndDate = null;
            }

            return dummyObj;
        }
    }
}
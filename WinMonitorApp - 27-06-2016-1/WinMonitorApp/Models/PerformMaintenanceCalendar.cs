using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Net;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace WinMonitorApp.Models
{
    public class PerformMaintenanceCalendar
    {
        // To generate sequences for Event Id
        public int getseqDBCalEventId()
        {
            WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
            int intnextseqDBEventId = mDBContext.Database.SqlQuery<int>("select next value for seqDBEventId").FirstOrDefault();
            return intnextseqDBEventId;
        }


        //Admin Maintenance Calendar

        //AddEVent In Calendar
        public string mSaveCalendarEvent(string pstrTitle, string pstrDetails, string pstrStartTime, string pstrEndTime, string pstrEventFor, string pstrMaintenance, int pstrCompanyId)
        {
            try
            {
                WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                if (pstrEventFor == "all")
                {
                    List<DBCompany> companyList = new List<DBCompany>();
                    companyList = (from company in mDBContext.DBCompanies
                                   select company).ToList();
                    foreach (DBCompany companyObj in companyList)
                    {
                        DBCalendar CalObj = new DBCalendar();
                        CalObj.DBEventId = getseqDBCalEventId();
                        CalObj.DBEventTitle = pstrTitle;
                        CalObj.DBEventDetails = pstrDetails;

                        DateTime mdateStartTime = DateTime.Parse(pstrStartTime);
                        CalObj.DBEventStartTime = mdateStartTime;

                        DateTime mdateEndTime = DateTime.Parse(pstrEndTime);
                        CalObj.DBEventEndTime = mdateEndTime;

                        CalObj.DBEventDifferenceTime = mdateEndTime.Subtract(mdateStartTime).ToString();

                        CalObj.DBEventMaintenance = pstrMaintenance;

                        CalObj.DBCompanyId = companyObj.DBCompanyId;

                        TimeSpan mstrRecordState = mdateEndTime.Subtract(DateTime.UtcNow);
                        if (mstrRecordState.TotalMinutes > 0)
                        {
                            CalObj.DBEventStatus = "Active";
                        }
                        else
                        {
                            CalObj.DBEventStatus = "Inactive";
                        }


                        mDBContext.DBCalendars.Add(CalObj);
                        mDBContext.SaveChanges();

                        List<DBSubscription> subscriptionObjList = new List<DBSubscription>();
                        subscriptionObjList = (from subObj in mDBContext.DBSubscriptions
                                               where subObj.DBCompanyId == companyObj.DBCompanyId
                                               select subObj).ToList();
                        DBEmailPage emailPageObj = new DBEmailPage();
                        emailPageObj = (from emailPage in mDBContext.DBEmailPages
                                        where emailPage.DBEmailPageId == 1
                                        select emailPage).FirstOrDefault();
                        foreach (DBSubscription subscriptionObj in subscriptionObjList)
                        {
                            PerformSubscription performSubscriptionObj = new PerformSubscription();
                            WebClient clientObj = new WebClient();
                            int endIndex, startIndex;
                            string stringHtml = emailPageObj.DBEmailContent.ToString();

                            endIndex = stringHtml.IndexOf("<!--end of componentUpdateDiv-->");
                            startIndex = stringHtml.IndexOf("<div id=\"divForComponentUpdates\">");
                            string stringToBeReplaced = stringHtml.Substring(startIndex, endIndex - startIndex);
                            stringHtml = stringHtml.Replace(stringToBeReplaced, "");

                            string link = "http://cha-en-pdp2:2108/?pUserCompanyName=CompanyNameVariable";                              //change url to direct to user page
                            stringHtml = stringHtml.Replace("linkToBeChanged", link);
                            stringHtml = stringHtml.Replace("CompanyNameVariable", companyObj.DBCompanyName);
                            stringHtml = stringHtml.Replace("customerNameVariable", subscriptionObj.DBName);
                            stringHtml = stringHtml.Replace("EventNameVariable", CalObj.DBEventTitle);
                            stringHtml = stringHtml.Replace("EventDetailsVariable", CalObj.DBEventDetails);
                            stringHtml = stringHtml.Replace("EventStartTimeVariable", CalObj.DBEventStartTime.ToString());
                            stringHtml = stringHtml.Replace("EventEndTimeVariable", CalObj.DBEventEndTime.ToString());
                            performSubscriptionObj.sendEmailForCalenderEvent( CalObj, subscriptionObj.DBEmail, null, "New Event Scheduled", stringHtml, CalObj.DBEventDetails, DateTime.Parse(pstrStartTime), DateTime.Parse(pstrEndTime), "CreateNew");
                        }
                    }
                }
                else
                {
                    DBCalendar CalObj = new DBCalendar();
                    CalObj.DBEventId = getseqDBCalEventId();
                    CalObj.DBEventTitle = pstrTitle;
                    CalObj.DBEventDetails = pstrDetails;

                    DateTime mdateStartTime = DateTime.Parse(pstrStartTime);
                    CalObj.DBEventStartTime = mdateStartTime;

                    DateTime mdateEndTime = DateTime.Parse(pstrEndTime);
                    CalObj.DBEventEndTime = mdateEndTime;

                    CalObj.DBEventDifferenceTime = mdateEndTime.Subtract(mdateStartTime).ToString();

                    //CalObj.DBEventRepetition = pstrEventFor;
                    CalObj.DBEventMaintenance = pstrMaintenance;

                    CalObj.DBCompanyId = pstrCompanyId;

                    TimeSpan mstrRecordState = mdateEndTime.Subtract(DateTime.UtcNow);
                    if (mstrRecordState.TotalMinutes > 0)
                    {
                        CalObj.DBEventStatus = "Active";
                    }
                    else
                    {
                        CalObj.DBEventStatus = "Inactive";
                    }


                    mDBContext.DBCalendars.Add(CalObj);
                    mDBContext.SaveChanges();

                    DBCompany companyObj = new DBCompany();
                    companyObj = (from company in mDBContext.DBCompanies
                                  where company.DBCompanyId == pstrCompanyId
                                  select company).FirstOrDefault();
                    List<DBSubscription> subscriptionObjList = new List<DBSubscription>();
                    subscriptionObjList = (from subObj in mDBContext.DBSubscriptions
                                           where subObj.DBCompanyId == pstrCompanyId
                                           select subObj).ToList();
                    DBEmailPage emailPageObj = new DBEmailPage();
                    emailPageObj = (from emailPage in mDBContext.DBEmailPages
                                    where emailPage.DBEmailPageId == 1
                                    select emailPage).FirstOrDefault();
                    foreach (DBSubscription subscriptionObj in subscriptionObjList)
                    {
                        PerformSubscription performSubscriptionObj = new PerformSubscription();
                        WebClient clientObj = new WebClient();
                        int endIndex, startIndex;
                        string stringHtml = emailPageObj.DBEmailContent.ToString();

                        endIndex = stringHtml.IndexOf("<!--end of componentUpdateDiv-->");
                        startIndex = stringHtml.IndexOf("<div id=\"divForComponentUpdates\">");
                        string stringToBeReplaced = stringHtml.Substring(startIndex, endIndex - startIndex);
                        stringHtml = stringHtml.Replace(stringToBeReplaced, "");

                        string link = "http://cha-en-pdp2:2108/?pUserCompanyName=CompanyNameVariable";                              //change url to direct to user page
                        stringHtml = stringHtml.Replace("linkToBeChanged", link);
                        stringHtml = stringHtml.Replace("CompanyNameVariable", companyObj.DBCompanyName);
                        stringHtml = stringHtml.Replace("customerNameVariable", subscriptionObj.DBName);
                        stringHtml = stringHtml.Replace("EventNameVariable", CalObj.DBEventTitle);
                        stringHtml = stringHtml.Replace("EventDetailsVariable", CalObj.DBEventDetails);
                        stringHtml = stringHtml.Replace("EventStartTimeVariable", CalObj.DBEventStartTime.ToString());
                        stringHtml = stringHtml.Replace("EventEndTimeVariable", CalObj.DBEventEndTime.ToString());
                        performSubscriptionObj.sendEmailForCalenderEvent(CalObj, subscriptionObj.DBEmail, null, "New Event Scheduled", stringHtml, CalObj.DBEventDetails, DateTime.Parse(pstrStartTime), DateTime.Parse(pstrEndTime), "CreateNew");
                    }
                }



                return "Event Saved Sucessfully!!";
            }
            catch (DbUpdateException exUpdateDB)
            {
                Console.Write(exUpdateDB);
                return "DbUpdateException";
            }
            catch (DbEntityValidationException exEntityValidateDB)
            {
                Console.Write(exEntityValidateDB);
                return "DbEntityValidationException";
            }
            catch (NotSupportedException exNotSupportedDB)
            {
                Console.Write(exNotSupportedDB);
                return "NotSupportedException";
            }
            catch (ObjectDisposedException exObjectDisposedDB)
            {
                Console.Write(exObjectDisposedDB);
                return "ObjectDisposedException";
            }
            catch (InvalidOperationException exInvalidOperationDB)
            {
                Console.Write(exInvalidOperationDB);
                return "InvalidOperationException";
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return "Misllaneous Exception";
            }
        }

        //Show Event Details on Event Click
        public List<DummyCalendar> mGetEventCalendarDetails(int pstrgetId)
        {
            WinMonitorEntityModelContext mDBContextObj = new WinMonitorEntityModelContext();
            return mDBContextObj.Database.SqlQuery<DummyCalendar>("select DBEventTitle as Title, DBEventDetails as Details, DBEventStartTime as Start, DBEventEndTime as EndTime, DBEventMaintenance as Maintenance, DBEventStatus as Status from DBCalendar where DBEventId=" + pstrgetId + "").ToList();
        }

        //Delete Event in Event Click
        public string mDeleteEvent(int pstrgetId)
        {
            try
            {
                WinMonitorEntityModelContext mDBContextObj = new WinMonitorEntityModelContext();
                DBCalendar CalObj = new DBCalendar();
                CalObj = (from calObj in mDBContextObj.DBCalendars
                          where calObj.DBEventId == pstrgetId
                          select calObj).FirstOrDefault();

                mDBContextObj.DBCalendars.Remove(CalObj);
                mDBContextObj.SaveChanges();
                return "Event Sucessfully Deleted!!";
            }
            catch (DbUpdateException exUpdateDB)
            {
                Console.Write(exUpdateDB);
                return "DbUpdateException";
            }
            catch (DbEntityValidationException exEntityValidateDB)
            {
                Console.Write(exEntityValidateDB);
                return "DbEntityValidationException";
            }
            catch (NotSupportedException exNotSupportedDB)
            {
                Console.Write(exNotSupportedDB);
                return "NotSupportedException";
            }
            catch (ObjectDisposedException exObjectDisposedDB)
            {
                Console.Write(exObjectDisposedDB);
                return "ObjectDisposedException";
            }
            catch (InvalidOperationException exInvalidOperationDB)
            {
                Console.Write(exInvalidOperationDB);
                return "InvalidOperationException";
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return "Misllaneous Exception";
            }
        }

        //function to load existing events from db
        public List<Events> gettingExistingEvents(int pstrCompanyId)
        {
            List<Events> eventList = new List<Events>();
            WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
            List<DBCalendar> calenderList = new List<DBCalendar>();
            calenderList = (from calenderEvent in contextObj.DBCalendars
                            where calenderEvent.DBCompanyId == pstrCompanyId
                            select calenderEvent).ToList();

            foreach (DBCalendar calenderObj in calenderList)
            {
                Events eventObj = new Events();
                eventObj.id = calenderObj.DBEventId;
                eventObj.title = calenderObj.DBEventTitle;
                eventObj.start = calenderObj.DBEventStartTime.ToString("s");
                eventObj.end = calenderObj.DBEventEndTime.ToString("s");
                eventObj.allDay = false;
                if (calenderObj.DBEventMaintenance == "Maintenance")
                    eventObj.backgroundColor = "#CCCCFF";
                else
                    eventObj.backgroundColor = "#9CEDFE";

                eventList.Add(eventObj);
            }


            return eventList;
        }


        //function to update events in Database
        public void UpdatingCalenderInDB(int pstrEventId, DateTime pstrNewEventStart, DateTime pstrNewEventEnd)
        {
            WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
            DBCalendar calenderObj = new DBCalendar();
            calenderObj = (from calenderEvent in contextObj.DBCalendars
                           where calenderEvent.DBEventId == pstrEventId
                           select calenderEvent).FirstOrDefault();
            if (calenderObj.DBEventStatus == "Active")
            {
                calenderObj.DBEventStartTime = pstrNewEventStart;
                calenderObj.DBEventEndTime = pstrNewEventEnd;
                contextObj.SaveChanges();
                DBCompany companyObj = new DBCompany();
                companyObj = (from company in contextObj.DBCompanies
                              where company.DBCompanyId == calenderObj.DBCompanyId
                              select company).FirstOrDefault();
                List<DBSubscription> subscriptionObjList = new List<DBSubscription>();
                subscriptionObjList = (from subObj in contextObj.DBSubscriptions
                                       where subObj.DBCompanyId == calenderObj.DBCompanyId
                                       select subObj).ToList();
                DBCalendar calEvent = new DBCalendar();
                calEvent = (from calObj in contextObj.DBCalendars
                            where calObj.DBEventId == pstrEventId
                            select calObj).FirstOrDefault();
                DBEmailPage emailPageObj = new DBEmailPage();
                emailPageObj = (from emailPage in contextObj.DBEmailPages
                                where emailPage.DBEmailPageId == 1
                                select emailPage).FirstOrDefault();
                foreach (DBSubscription subscriptionObj in subscriptionObjList)
                {
                    PerformSubscription performSubscriptionObj = new PerformSubscription();
                    WebClient clientObj = new WebClient();
                    int endIndex, startIndex;
                    string stringHtml = emailPageObj.DBEmailContent.ToString();

                    endIndex = stringHtml.IndexOf("<!--end of componentUpdateDiv-->");
                    startIndex = stringHtml.IndexOf("<div id=\"divForComponentUpdates\">");
                    string stringToBeReplaced = stringHtml.Substring(startIndex, endIndex - startIndex);
                    stringHtml = stringHtml.Replace(stringToBeReplaced, "");

                    string link = "http://cha-en-pdp2:2108/?pUserCompanyName=CompanyNameVariable";                              //change url to direct to user page
                    stringHtml = stringHtml.Replace("linkToBeChanged", link);
                    stringHtml = stringHtml.Replace("CompanyNameVariable", companyObj.DBCompanyName);
                    stringHtml = stringHtml.Replace("customerNameVariable", subscriptionObj.DBName);
                    stringHtml = stringHtml.Replace("EventNameVariable", calEvent.DBEventTitle);
                    stringHtml = stringHtml.Replace("EventDetailsVariable", calEvent.DBEventDetails);
                    stringHtml = stringHtml.Replace("EventStartTimeVariable", calEvent.DBEventStartTime.ToString());
                    stringHtml = stringHtml.Replace("EventEndTimeVariable", calEvent.DBEventEndTime.ToString());
                    performSubscriptionObj.sendEmailForCalenderEvent(calenderObj, subscriptionObj.DBEmail, null, "Event Schedule Updated", stringHtml, calEvent.DBEventDetails, pstrNewEventStart, pstrNewEventEnd, "Update");
                }
            }
        }

        //function to inactivate events
        public void InactivatingEvents()
        {
            WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
            List<DBCalendar> calenderEventList = contextObj.DBCalendars.ToList();
            foreach (DBCalendar calEvent in calenderEventList)
            {
                TimeSpan mstrRecordState = calEvent.DBEventEndTime.Subtract(DateTime.UtcNow);
                if (mstrRecordState.TotalMinutes < 0)
                {
                    calEvent.DBEventStatus = "Inactive";
                }
                contextObj.SaveChanges();
            }

        }
    }
}
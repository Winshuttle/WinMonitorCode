using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace WinMonitorApp.Models
{
    public class PerformMaintenanceCalendar
    {
        // To generate sequences for Event Id
        public string getseqDBCompanyId()
        {
            WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
            string intnextseqDBEventId = "Cal" + mDBContext.Database.SqlQuery<int>("select next value for seqDBEventId").FirstOrDefault().ToString();
            return intnextseqDBEventId;
        }


        //Admin Maintenance Calendar

        //AddEVent In Calendar
        public string mSaveCalendarEvent(string pstrTitle, string pstrDetails, string pstrStartTime, string pstrEndTime, string pstrRepetition, string pstrMaintenance, string pstrCompanyId)
        {
            try
            {
                WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                DBCalendar CalObj = new DBCalendar();
                CalObj.DBEventId = getseqDBCompanyId();
                CalObj.DBEventTitle = pstrTitle;
                CalObj.DBEventDetails = pstrDetails;

                DateTime mdateStartTime = DateTime.Parse(pstrStartTime);
                CalObj.DBEventStartTime = mdateStartTime;

                DateTime mdateEndTime = DateTime.Parse(pstrEndTime);
                CalObj.DBEventEndTime = mdateEndTime;

                CalObj.DBEventDifferenceTime = mdateEndTime.Subtract(mdateStartTime).ToString();

                CalObj.DBEventRepetition = pstrRepetition;
                CalObj.DBEventMaintenance = pstrMaintenance;

                CalObj.DBCompanyId = pstrCompanyId;

                TimeSpan mstrRecordState = mdateEndTime.Subtract(DateTime.Now);
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
        public List<DummyCalendar> mGetEventCalendarDetails(string pstrgetId)
        {
            WinMonitorEntityModelContext mDBContextObj = new WinMonitorEntityModelContext();
            return mDBContextObj.Database.SqlQuery<DummyCalendar>("select DBEventTitle as Title, DBEventDetails as Details, DBEventStartTime as Start, DBEventEndTime as EndTime, DBEventRepetition as Repetition, DBEventMaintenance as Maintenance, DBEventStatus as Status from DBCalendar where DBEventId='" + pstrgetId + "'").ToList();
        }

        //Delete Event in Event Click
        public string mDeleteEvent(string pstrgetId)
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
        public List<Events> gettingExistingEvents(string pstrCompanyId)
        {
            List<Events> eventList = new List<Events>();
            WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
            List<DBCalendar> calenderList = new List<DBCalendar>();
            calenderList = (from calenderEvent in contextObj.DBCalendars
                            where calenderEvent.DBCompanyId==pstrCompanyId
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
                else if (calenderObj.DBEventMaintenance == "Performance Degradation")
                    eventObj.backgroundColor = "#FFFFD3";
                else if (calenderObj.DBEventMaintenance == "Service Disruption")
                    eventObj.backgroundColor = "#FFC6C6";
                else
                    eventObj.backgroundColor = "CCFFD9";

                eventList.Add(eventObj);
            }


            return eventList;
        }


        //function to update events in Database
        public void UpdatingCalenderInDB(string pstrEventId, DateTime pstrNewEventStart, DateTime pstrNewEventEnd)
        {
            WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
            DBCalendar calenderObj = new DBCalendar();
            calenderObj = (from calenderEvent in contextObj.DBCalendars
                           where calenderEvent.DBEventId == pstrEventId
                           select calenderEvent).FirstOrDefault();
            calenderObj.DBEventStartTime = pstrNewEventStart;
            calenderObj.DBEventEndTime = pstrNewEventEnd;
            contextObj.SaveChanges();
        }

        //function to inactivate events
        public void InactivatingEvents() 
        {
            WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
            List<DBCalendar> calenderEventList = contextObj.DBCalendars.ToList();
            foreach (DBCalendar calEvent in calenderEventList)
            {
                TimeSpan mstrRecordState = calEvent.DBEventEndTime.Subtract(DateTime.Now);
                if (mstrRecordState.TotalMinutes < 0)
                {
                    calEvent.DBEventStatus = "Inactive";
                }
                contextObj.SaveChanges();
            }

        }


        /* SAMPLE FUNCTIONS
         
          public void InactiveEvents();
          
          public void UpdateEvent();
          
          public void DeleteEvent();
          
          public void AddEvent();
          
         */



        //User Maintenance Calendar

        


    }
}
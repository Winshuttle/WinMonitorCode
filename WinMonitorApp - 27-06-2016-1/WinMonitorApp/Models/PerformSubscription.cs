using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using Microsoft.Exchange.WebServices.Data;

namespace WinMonitorApp.Models
{
    public class PerformSubscription
    {
        // To generate sequences from the self generated sequences in the database
        public int getseqDBSubscriptionsId()
        {
            WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
            int intnextseqDBSubscriptionsId = mDBContext.Database.SqlQuery<int>("select next value for seqDBSubscriptionsId").FirstOrDefault();
            return intnextseqDBSubscriptionsId;
        }

        //method to add subscriptions in the database
        public string mAddSubscription(string pstrName, string pstrEmail, int pstrId)
        {
            WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
            DBSubscription SubscriptionObj = new DBSubscription();
            SubscriptionObj.DBSubscriptionsId = getseqDBSubscriptionsId();
            SubscriptionObj.DBName = pstrName;
            SubscriptionObj.DBEmail = pstrEmail;
            SubscriptionObj.DBCompanyId = pstrId;

            int limitSubscriptionsPerCompany = mDBContext.Database.SqlQuery<Int32>("select count(*) from DBSubscriptions where DBCompanyId='"+ pstrId +"';").FirstOrDefault();

            try
            {
                if(limitSubscriptionsPerCompany <5)
                {
                mDBContext.DBSubscriptions.Add(SubscriptionObj);
                mDBContext.SaveChanges();
                return "Sucess, Subscriptions left for company:'"+ (4-limitSubscriptionsPerCompany) +"'";
                }
                else
                {
                    return "Unsucessful: Subscriptions limit: 5 for company reached";
                }
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

        //function to count subscriptions
        public string getSubscriptionCount()
        {
            WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();

            try
            {
                int countSubscriptions = mDBContext.Database.SqlQuery<Int32>("select count(*) from DBSubscriptions;").FirstOrDefault();
                return countSubscriptions.ToString();
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


        //function to send email of subscription
        public void sendEmail(string pstrRecipientsEmailId, string pstrCCRecipientsEmailId, string pstrSubject, string pstrMessageBody)
        {
            ExchangeService objExchangeService = new ExchangeService(ExchangeVersion.Exchange2013);

            objExchangeService.Credentials = new WebCredentials("wse\\centraluser", "$abcd1234");
            objExchangeService.UseDefaultCredentials = false;
            objExchangeService.Url = new Uri("https://mail.winshuttle.in/EWS/Exchange.asmx");
            EmailMessage objMessage = new EmailMessage(objExchangeService);
            objMessage.ToRecipients.Add(pstrRecipientsEmailId);
            if (pstrCCRecipientsEmailId != null)
            {
                objMessage.CcRecipients.Add(pstrCCRecipientsEmailId);
            }
            objMessage.Subject = pstrSubject;
            objMessage.ReplyTo.Add(new EmailAddress("donotreply@winshuttle.com"));
            objMessage.Body = new MessageBody(pstrMessageBody);
            objMessage.Body.BodyType = BodyType.HTML;
            objMessage.Send();
        }


        //function to send email for calender event subscription
        public void sendEmailForCalenderEvent(DBCalendar calObj, string pstrRecipientsEmailId, string pstrCCRecipientsEmailId, string pstrSubject, string pstrMessageBody, string pstrMessage, DateTime startTime, DateTime endTime, string actionToBeTaken)
        {
            ExchangeService objExchangeService = new ExchangeService(ExchangeVersion.Exchange2013);
            objExchangeService.Credentials = new WebCredentials("wse\\centraluser", "$abcd1234");
            objExchangeService.UseDefaultCredentials = false;
            objExchangeService.Url = new Uri("https://mail.winshuttle.in/EWS/Exchange.asmx");
            EmailMessage objMessage = new EmailMessage(objExchangeService);
            objMessage.ToRecipients.Add(pstrRecipientsEmailId);
            if (pstrCCRecipientsEmailId != null)
            {
                objMessage.CcRecipients.Add(pstrCCRecipientsEmailId);
            }
            objMessage.Subject = pstrSubject;
            objMessage.ReplyTo.Add(new EmailAddress("donotreply@winshuttle.com"));
            objMessage.Body = new MessageBody(pstrMessageBody);
            objMessage.Body.BodyType = BodyType.HTML;


            Appointment appObj;
            if (actionToBeTaken == "Update")
            {


                WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                DateTime endDateLimit = contextObj.Database.SqlQuery<DateTime>("select DBEventEndTime from DBCalendar").Last();

                CalendarFolder folder = CalendarFolder.Bind(objExchangeService, WellKnownFolderName.Calendar);
                CalendarView view = new CalendarView(DateTime.UtcNow, endDateLimit);

                FindItemsResults<Appointment> results = folder.FindAppointments(view);

                foreach (Appointment appointment in  results)
                {
                    if ((appointment.Subject == calObj.DBEventTitle) || (appointment.Start == calObj.DBEventStartTime) || (appointment.End == calObj.DBEventEndTime))
                    {
                        appObj = Appointment.Bind(objExchangeService, appointment.Id, new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Body, AppointmentSchema.Start, AppointmentSchema.End));
                        appObj.Subject = pstrSubject;
                        appObj.Body = pstrMessage;
                        appObj.Start = startTime;
                        appObj.End = endTime;
                        appObj.RequiredAttendees.Add(pstrRecipientsEmailId);
                        appObj.Update(ConflictResolutionMode.AlwaysOverwrite, SendInvitationsOrCancellationsMode.SendToAllAndSaveCopy);
                    }
                }
                
            }
            else
            if (actionToBeTaken == "CreateNew")
            {
                appObj = new Appointment(objExchangeService);
                appObj.Subject = calObj.DBEventTitle;
                appObj.Body = calObj.DBEventDetails;
                appObj.Start = calObj.DBEventStartTime;
                appObj.End = calObj.DBEventEndTime;
                appObj.RequiredAttendees.Add(pstrRecipientsEmailId);
                appObj.Save(SendInvitationsMode.SendToAllAndSaveCopy);

            }
            else {
                WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                DateTime endDateLimit = DateTime.Parse(contextObj.Database.SqlQuery<string>("select DBEventEndTime from DBCalendar").Last());

                CalendarFolder folder = CalendarFolder.Bind(objExchangeService, WellKnownFolderName.Calendar);
                CalendarView view = new CalendarView(DateTime.UtcNow, endDateLimit);

                FindItemsResults<Appointment> results = folder.FindAppointments(view);

                foreach (Appointment appointment in results)
                {
                    if ((appointment.Subject == calObj.DBEventTitle) || (appointment.Start == calObj.DBEventStartTime) || (appointment.End == calObj.DBEventEndTime))
                    {
                        appObj = Appointment.Bind(objExchangeService, appointment.Id, new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Body, AppointmentSchema.Start, AppointmentSchema.End));
                        appObj.Delete(DeleteMode.HardDelete);
                    }
                }

            }
            
            objMessage.Send();
        }


    }
}
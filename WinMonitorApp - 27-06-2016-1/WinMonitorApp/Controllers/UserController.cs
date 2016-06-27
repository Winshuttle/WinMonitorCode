using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WinMonitorApp.Models;

namespace WinMonitorApp.Controllers
{
    public class UserController : Controller
    {

        //returns companyname from url
        public ActionResult userMasterPage(string pUserCompanyName)
        {
            if (pUserCompanyName != null)
            {
                PerformCompanyComponentWithStatusIncident PerformCompanyComponentWithStatusCompanyIdObj = new PerformCompanyComponentWithStatusIncident();
                if (PerformCompanyComponentWithStatusCompanyIdObj.mGetUserCompanyId(pUserCompanyName) !=0)
                 {
                     ViewBag.mUserCompanyId = PerformCompanyComponentWithStatusCompanyIdObj.mGetUserCompanyId(pUserCompanyName);
                    ViewBag.mUserCompanyName = pUserCompanyName;
                    return View(ViewBag);
                 }
                else
                    return null;
            }
            else
                return null;
        }
       

        //json to display status page
        public JsonResult jsonStatusUserView(string CompanyId)
        {
            PerformCompanyComponentWithStatusIncident PerformStatusObj = new PerformCompanyComponentWithStatusIncident();
            List<DBComponent_With_Status> ModelStatus = PerformStatusObj.mGetStatus(Int32.Parse(CompanyId));
            return Json(ModelStatus,JsonRequestBehavior.AllowGet);
        }

        
        //sumbit subscription details to the database company wise ->limited to 5 users per company
        public string jsonAddSubscriptions(string pstrSubscriptionName, string pstrSubscriptionEmail, string pstrCompanyId)
        {
            PerformSubscription PerformAddSubscriptionObj = new PerformSubscription();
            string returnMessage = PerformAddSubscriptionObj.mAddSubscription(pstrSubscriptionName, pstrSubscriptionEmail, Int32.Parse(pstrCompanyId));
            return returnMessage;
        }

        //render status and subscriptions view
        public ActionResult statusUserView(string commonCheck)
        {
            if (commonCheck == "CheckIfTrueErrorWin")
            {
                return View();
            }
            else
            {
                return null;
            }
        }


        
        public JsonResult jsonHistoryUserView(string mHistoryCompanyId)
        {
            PerformCompanyComponentWithStatusIncident PerformHistoryObj = new PerformCompanyComponentWithStatusIncident();
            List<DummyHistory> ModelHistory = PerformHistoryObj.mGetHistory(Int32.Parse(mHistoryCompanyId)); 
            return Json(ModelHistory, JsonRequestBehavior.AllowGet);
        }

        //render history view
        public ActionResult historyUserView(string commonCheck)
        {
            if (commonCheck == "CheckIfTrueErrorWin")
            {
                return View();
            }
            else
            {
                return null;
            }
        }

        //display read only calendar
        public ActionResult MaintenanceCalendar(string commonCheck)
        {
            if (commonCheck == "CheckIfTrueErrorWin")
            {
                return View();
            }
            else
            {
                return null;
            }
        }

        //to load existing events
        public JsonResult getExistingEventsInUser(string pstrCompanyId)
        {

            PerformMaintenanceCalendar performObj = new PerformMaintenanceCalendar();
            var events = (performObj.gettingExistingEvents(Int32.Parse(pstrCompanyId))).ToArray();
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        //Event ClickShow        --COMBINED
        public JsonResult jsonEventClickInUser(string jsonpstrgetEventId)
        {
            PerformMaintenanceCalendar performEventClickObj = new PerformMaintenanceCalendar();
            List<DummyCalendar> modelDetails = performEventClickObj.mGetEventCalendarDetails(Int32.Parse(jsonpstrgetEventId));                                                             // to be changed
            return Json(modelDetails, JsonRequestBehavior.AllowGet);
        }

        //render metrics view
        public ActionResult Metrics(string commonCheck)
        {
            if (commonCheck == "CheckIfTrueErrorWin")
            {
                return View();
            }
            else
            {
                return null;
            }
        }

        public ActionResult Performance(string commonCheck)
        {
            if (commonCheck == "CheckIfTrueErrorWin")
            {
                return View();
            }
            else
            {
                return null;
            }
        }

        //controller function to get company details
        public JsonResult getCompanyDetails(string pIntCompanyId)
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            return Json(performObj.getCompanyDetailsById(Int32.Parse(pIntCompanyId)), JsonRequestBehavior.AllowGet);
        }


        //controller to get all components related to company
        public JsonResult getComponents(string pstrCompanyId)
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            return Json(performObj.gettingComponents(Int32.Parse(pstrCompanyId)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getEventTimeStored(string pstrComponentId, string pstrEventStartOffset, string pstrEventEndOffset, string pstrRecentOrNot)
        {
            PerformUserPerformance performObj = new PerformUserPerformance();
            return Json(performObj.gettingEventTimeStored(Int32.Parse(pstrComponentId), Int32.Parse(pstrEventStartOffset), Int32.Parse(pstrEventEndOffset), pstrRecentOrNot), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getDetailsOfIncident(string pstrLogId) 
        {
            PerformUserPerformance performObj = new PerformUserPerformance();
            return Json(performObj.gettingDetailsOfIncident(Int32.Parse(pstrLogId)), JsonRequestBehavior.AllowGet);
        }
        public void InactivateEventsInUser()
        {
            PerformMaintenanceCalendar performObj = new PerformMaintenanceCalendar();
            performObj.InactivatingEvents();
        }


        //controller function to get incident details of component
        public JsonResult getIncidentDetailsOfComponent(string pstrComponentId)
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            return Json(performObj.gettingIncidentDetailsOfComponent(Int32.Parse(pstrComponentId)));
        }

        //controller function to get expected end time of incident
        public JsonResult getExpectedEndTime(string pstrComponentId)
        {
            PerformUserPerformance performObj = new PerformUserPerformance();
            return Json(performObj.gettingExpectedEndTime(Int32.Parse(pstrComponentId)));
        }

    }
}

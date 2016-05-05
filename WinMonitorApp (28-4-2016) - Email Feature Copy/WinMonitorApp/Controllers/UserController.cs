using System;
using System.Collections.Generic;
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
            PerformCompanyComponentWithStatusIncident PerformCompanyComponentWithStatusCompanyIdObj = new PerformCompanyComponentWithStatusIncident();
            ViewBag.mUserCompanyId = PerformCompanyComponentWithStatusCompanyIdObj.mGetUserCompanyId(pUserCompanyName);
            ViewBag.mUserCompanyName = pUserCompanyName;
            return View(ViewBag);
        }
       

        //class to show details on status page
        public class jsonStatusServer
        {
            public string jsonCompanyId { get; set; }
        }
   
        //json to display status page
        public JsonResult jsonStatusUserView(jsonStatusServer jsonStatusServers)
        {
            string mStatusCompanyId = jsonStatusServers.jsonCompanyId;
            PerformCompanyComponentWithStatusIncident PerformStatusObj = new PerformCompanyComponentWithStatusIncident();
            List<GetStatus> ModelStatus = PerformStatusObj.mGetStatus(mStatusCompanyId);
            return Json(ModelStatus,JsonRequestBehavior.AllowGet);
        }

        
        //sumbit subscription details to the database company wise ->limited to 5 users per company
        public string jsonAddSubscriptions(string pstrSubscriptionName, string pstrSubscriptionEmail, string pstrCompanyId)
        {
            PerformSubscription PerformAddSubscriptionObj = new PerformSubscription();
            string returnMessage = PerformAddSubscriptionObj.mAddSubscription(pstrSubscriptionName, pstrSubscriptionEmail, pstrCompanyId);
            return returnMessage;
        }

        //render status and subscriptions view
        public ActionResult statusUserView()
        {
            return View();
        }










        //show details on history page
        public class jsonHistoryServer
        {
            public string jsonCompanyHistoryId { get; set; }
        }
        public JsonResult jsonHistoryUserView(jsonHistoryServer jsonHistoryServers)
        {
            string mHistoryCompanyId = jsonHistoryServers.jsonCompanyHistoryId;
            PerformCompanyComponentWithStatusIncident PerformHistoryObj = new PerformCompanyComponentWithStatusIncident();
            List<DummyHistory> ModelHistory = PerformHistoryObj.mGetHistory(mHistoryCompanyId); 
            return Json(ModelHistory, JsonRequestBehavior.AllowGet);
        }

        //render history view
        public ActionResult historyUserView()
        {
            return View();
        }

        //display read only calendar
        public ActionResult MaintenanceCalendar()
        {
            return View();
        }

        //to load existing events
        public JsonResult getExistingEventsInUser(string pstrCompanyId)
        {

            PerformMaintenanceCalendar performObj = new PerformMaintenanceCalendar();
            var events = (performObj.gettingExistingEvents(pstrCompanyId)).ToArray();
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        //render metrics view
        public ActionResult Metrics()
        {
            return View();
        }

        public ActionResult Performance()
        {
            return View();
        }

    }
}

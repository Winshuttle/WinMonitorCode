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

        public ActionResult userMasterPage(string pUserCompanyName)
        {
            PerformCompanyComponentWithStatusIncident PerformCompanyComponentWithStatusCompanyIdObj = new PerformCompanyComponentWithStatusIncident();
            ViewBag.mUserCompanyId = PerformCompanyComponentWithStatusCompanyIdObj.mGetUserCompanyId(pUserCompanyName);
            ViewBag.mUserCompanyName = pUserCompanyName;
            return View(ViewBag);
        }
       



        //showing details on status page
        public class jsonStatusServer
        {
            public string jsonCompanyId { get; set; }
        }
   
        public JsonResult jsonStatusUserView(jsonStatusServer jsonStatusServers)
        {
            string mStatusCompanyId = jsonStatusServers.jsonCompanyId;
            PerformCompanyComponentWithStatusIncident PerformStatusObj = new PerformCompanyComponentWithStatusIncident();
            List<GetStatus> ModelStatus = PerformStatusObj.mGetStatus(mStatusCompanyId);
            return Json(ModelStatus,JsonRequestBehavior.AllowGet);
        }

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

        public ActionResult historyUserView()
        {
            return View();
        }





        public ActionResult Subscription()
        {
            return View();
        }
        public ActionResult Metrics()
        {
            return View();
        }
    }
}

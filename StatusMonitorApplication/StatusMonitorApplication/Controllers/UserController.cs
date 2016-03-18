using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinMonitorApp.Controllers
{
    public class UserController : Controller
    {
        
        public ActionResult userMasterPage()
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
        public ActionResult statusUserView()
        {
            return View();
        }
        public ActionResult historyUserView()
        {
            return View();
        }

    }
}

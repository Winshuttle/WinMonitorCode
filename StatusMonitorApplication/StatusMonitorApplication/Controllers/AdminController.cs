using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinMonitorApp.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Master()
        {
            return View();
        }

        public ActionResult CompanyDashboard()
        {
            return View();
        }

        public ActionResult ComponentandStatus()
        {
            return View();
        }

        public ActionResult PerformanceMetrics()
        {
            return View();
        }

        public ActionResult ActivatePage()
        {
            return View();
        }

        public ActionResult RegisteredAdmin()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }
    }
}

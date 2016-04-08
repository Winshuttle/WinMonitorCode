using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WinMonitorApp.Models;

namespace WinMonitorApp.Controllers
{
    public class AdminController : Controller
    {
        //controller to display Master Page
        public ActionResult Master()
        {
            return View();
        }



        //controller to display Company Page
        public ActionResult CompanyDashboard()
        {
            PerformCompanyComponentWithStatusIncident PerformCompanyComponentWithStatusIncidentObj = new PerformCompanyComponentWithStatusIncident();
            List<DBCompany> ModelCompany = PerformCompanyComponentWithStatusIncidentObj.mRetrieveCompanyDetails();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            return View(ModelCompany);
        }



        //controller to display Component Page
        public ActionResult ComponentandStatus()
        {
            return View();
        }



        //controllet to display performance metrics page
        public ActionResult PerformanceMetrics()
        {
            return View();
        }



        //controller to display Activation page
        public ActionResult ActivatePage()
        {
            return View();
        }



        //controller to display RegisteredAdmin page
        public ActionResult RegisteredAdmin()
        {
            PerformLogin PerformLoginObj = new PerformLogin();
            List<DBLogin> ModelLogin = PerformLoginObj.mRetrieveLoginDetails();
            return View(ModelLogin);
        }



        //controller to display Settings page
        public ActionResult Settings()
        {
            return View();
        }



        //controller to return component details based on company name to component page
        public JsonResult ComponentandStatusFromDB(string pstrCompanyIdFromPage)
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            IEnumerable<DummyClassForComponentPageDisplay> resultObj = performObj.LoadExistingComponentsFromDataBase(pstrCompanyIdFromPage);
            return Json(resultObj, JsonRequestBehavior.AllowGet);
        }



        public JsonResult getMasterComponentDataFromDataBase()
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            IEnumerable<DBMaster_DBComponent_With_Status> resultObj = performObj.MasterComponentDataFromDB();
            return Json(resultObj, JsonRequestBehavior.AllowGet);
        }


        public string addMasterComponentToDB(string pstrMasterComponentListFromPage, string pstrCompanyId)
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            List<string> MasterComponentListFromPage = new List<string>();
            string[] distributedMasterComponents = pstrMasterComponentListFromPage.Split(new char[] { '&' });
            foreach (var masterComponentFromPage in distributedMasterComponents)
            {
                string[] componentProp = masterComponentFromPage.Split(new char[] { '=' });
                MasterComponentListFromPage.Add(componentProp[1]);
            }
            return (performObj.AddComponentToDb(MasterComponentListFromPage, pstrCompanyId));

        }


        public string jsonCompanyRetrieve(string pstrJsonCompanyName, string pstrJsonCompanyURL)
        {
            PerformCompanyComponentWithStatusIncident CompanyAddAccountObj = new PerformCompanyComponentWithStatusIncident();
            string catchExceptionCompany = CompanyAddAccountObj.mSaveAddCompanyDetails(pstrJsonCompanyName, pstrJsonCompanyURL);

            return catchExceptionCompany;
        }



        //perform Specific Component details save and display ajax operation
        public class jsonSpecificComponentServer
        {
            public string jsonSpecificComponentName { get; set; }
            public string jsonSpecificComponentStatus { get; set; }
            public string jsonSpecificComponentCompanyId { get; set; } //comes from global variable
        }

        public string jsonSpecificComponentRetrieve(jsonSpecificComponentServer jsonSpecificComponentServers)
        {
            string mSpecificComponentNameSend = jsonSpecificComponentServers.jsonSpecificComponentName.ToString();
            string mSpecificComponentStatusSend = jsonSpecificComponentServers.jsonSpecificComponentStatus.ToString();
            string mSpecificComponentCompanyId = jsonSpecificComponentServers.jsonSpecificComponentCompanyId.ToString();

            PerformCompanyComponentWithStatusIncident SpecificComponentAddObj = new PerformCompanyComponentWithStatusIncident();
            string catchExceptionSpecificComponent = SpecificComponentAddObj.mSaveAddSpecificComponentDetails(mSpecificComponentNameSend, mSpecificComponentStatusSend, mSpecificComponentCompanyId);

            return catchExceptionSpecificComponent;
        }


        //perform Incident details save and display ajax operation
        public class jsonIncidentServer
        {
            public string jsonIncidentName { get; set; }
            public string jsonIncidentDetails { get; set; }
            public List<string> jsonComponentIdList { get; set; }
            public string jsonCompanyID { get; set; }
        }

        public string jsonIncidentRetreive(jsonIncidentServer jsonIncidentServers)
        {
            string mIncidentNameSend = jsonIncidentServers.jsonIncidentName;
            string mIncidentDetailsSend = jsonIncidentServers.jsonIncidentDetails;
            List<string> mComponentIdList = jsonIncidentServers.jsonComponentIdList;
            string mCompanyId = jsonIncidentServers.jsonCompanyID;

            PerformCompanyComponentWithStatusIncident IncidentAddObj = new PerformCompanyComponentWithStatusIncident();
            string catchExceptionIncident = IncidentAddObj.mSaveAddIncidentDetails(mIncidentNameSend, mIncidentDetailsSend, mComponentIdList, mCompanyId);

            return catchExceptionIncident;
        }



        public string updateStatusOfSelectedComponents(string pstrListOfCheckBoxes, string pstrStatusSelected, string pstrCompanyId)
        {
            List<string> mListOfCheckBoxes = new List<string>();
            string strResult = string.Empty;
            string[] separatedCheckBoxes = pstrListOfCheckBoxes.Split(new char[] { '&' });
            foreach (string separateComponent in separatedCheckBoxes)
            {
                string[] componentProperty = separateComponent.Split(new char[] { '=' });
                mListOfCheckBoxes.Add(componentProperty[1]);
            }
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            strResult = performObj.UpdatingStatusOfComponentsSelected(mListOfCheckBoxes, pstrStatusSelected, pstrCompanyId);
            return (strResult);
        }

        public void deleteSelectedComponents(string pComponentList, string pCompanyId)
        {
            List<string> mListOfComponents = new List<string>();
            string strResult = string.Empty;
            string[] separatedCheckBoxes = pComponentList.Split(new char[] { '&' });
            foreach (string separateComponent in separatedCheckBoxes)
            {
                string[] componentProperty = separateComponent.Split(new char[] { '=' });
                mListOfComponents.Add(componentProperty[1]);
            }
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            performObj.DeleteSelectedComponents(mListOfComponents, pCompanyId);

        }

    }
}

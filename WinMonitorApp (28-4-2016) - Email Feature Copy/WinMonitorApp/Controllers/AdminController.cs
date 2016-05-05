using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WinMonitorApp.Models;


using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

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
            return View(ModelCompany);
        }


        //class for getCompanyDataCenter
        public class jsonCompanyCenter
        {
            public string GetCompanyId {get; set;}
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



        //controller to display Editable MaintenanceCalendar page
        public ActionResult EditMaintenanceCalendar()
        {
            return View();
        }

        //Day Click Event
        public string jsonAddDayClickEvents(string jsonpstrTitle, string jsonpstrDetails, string jsonpstrStartTime, string jsonpstrEndTime, string jsonpstrRepetition, string jsonpstrMaintenance, string jsonpstrCompanyId)
        {
            PerformMaintenanceCalendar performDayEventObj = new PerformMaintenanceCalendar();
            string resultString = performDayEventObj.mSaveCalendarEvent(jsonpstrTitle, jsonpstrDetails, jsonpstrStartTime, jsonpstrEndTime, jsonpstrRepetition, jsonpstrMaintenance, jsonpstrCompanyId);
            return resultString;
        }

        //Event ClickShow        --COMBINED 
        public JsonResult jsonEventClick(string jsonpstrgetEventId)
        {
            PerformMaintenanceCalendar performEventClickObj = new PerformMaintenanceCalendar();
            List<DummyCalendar> modelDetails = performEventClickObj.mGetEventCalendarDetails(jsonpstrgetEventId);                                                             // to be changed
            return Json(modelDetails, JsonRequestBehavior.AllowGet);
        }

        //Event ClickDelete      --COMBINED
        public string jsonEventClickDelete(string jsonpstrEventId)
        {
            PerformMaintenanceCalendar performEventDeleteObj = new PerformMaintenanceCalendar();
            string result = performEventDeleteObj.mDeleteEvent(jsonpstrEventId);
            return result;
        }

        //controller to inactivate Events
        public void InactivateEvents() 
        {
            PerformMaintenanceCalendar performObj = new PerformMaintenanceCalendar();
            performObj.InactivatingEvents();
        }


        //function to convert time to unixtime
        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        //function to get existing events from db
        public JsonResult getExistingEvents(string pstrCompanyId)
        {
            
            PerformMaintenanceCalendar performObj = new PerformMaintenanceCalendar();
            var events = (performObj.gettingExistingEvents(pstrCompanyId)).ToArray();
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        //function to update event in DB
        public void UpdateCalenderInDB(string pstrEventId, DateTime pstrNewEventStart, DateTime pstrNewEventEnd)
        {
            PerformMaintenanceCalendar performObj = new PerformMaintenanceCalendar();
            performObj.UpdatingCalenderInDB(pstrEventId, pstrNewEventStart, pstrNewEventEnd);
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


        //controller to return master component list from database
        public JsonResult getMasterComponentDataFromDataBase()
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            IEnumerable<DBMaster_DBComponent_With_Status> resultObj = performObj.MasterComponentDataFromDB();
            return Json(resultObj, JsonRequestBehavior.AllowGet);
        }


        //controller to add selected master components from page to company component list 
        public string addMasterComponentToDB(string pstrMasterComponentListFromPage, string pstrCompanyId, string pstrPrimaryDataCenterName)
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            List<string> MasterComponentListFromPage = new List<string>();
            string[] distributedMasterComponents = pstrMasterComponentListFromPage.Split(new char[] { '&' });
            foreach (var masterComponentFromPage in distributedMasterComponents)
            {
                string[] componentProp = masterComponentFromPage.Split(new char[] { '=' });
                MasterComponentListFromPage.Add(componentProp[1]);
            }
            return (performObj.AddComponentToDb(MasterComponentListFromPage, pstrCompanyId, pstrPrimaryDataCenterName));

        }


        //controller to add new company account by admin
        public string jsonCompanyRetrieve(string pstrJsonCompanyName, string pstrJsonCompanyURL, string pstrJsonPrimary, string pstrJsonSecondary)
        {
            PerformCompanyComponentWithStatusIncident CompanyAddAccountObj = new PerformCompanyComponentWithStatusIncident();
            string catchExceptionCompany = CompanyAddAccountObj.mSaveAddCompanyDetails(pstrJsonCompanyName, pstrJsonCompanyURL, pstrJsonPrimary, pstrJsonSecondary);

            return catchExceptionCompany;
        }



        //perform Specific Component details save and display ajax operation
        public class jsonSpecificComponentServer
        {
            public string jsonSpecificComponentName { get; set; }
            public string jsonSpecificComponentStatus { get; set; }
            public string jsonSpecificComponentCompanyId { get; set; } //comes from global variable
            public string jsonSpecificComponentDataCenter { get; set; }
        }

        public string jsonSpecificComponentRetrieve(jsonSpecificComponentServer jsonSpecificComponentServers)
        {
            string mSpecificComponentNameSend = jsonSpecificComponentServers.jsonSpecificComponentName.ToString();
            string mSpecificComponentStatusSend = jsonSpecificComponentServers.jsonSpecificComponentStatus.ToString();
            string mSpecificComponentCompanyId = jsonSpecificComponentServers.jsonSpecificComponentCompanyId.ToString();
            string mSpecificComponentDataCenter= jsonSpecificComponentServers.jsonSpecificComponentDataCenter.ToString();

            PerformCompanyComponentWithStatusIncident SpecificComponentAddObj = new PerformCompanyComponentWithStatusIncident();
            string catchExceptionSpecificComponent = SpecificComponentAddObj.mSaveAddSpecificComponentDetails(mSpecificComponentNameSend, mSpecificComponentStatusSend, mSpecificComponentCompanyId, mSpecificComponentDataCenter);

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


        //controller to update the status of selected components
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

        //controller to delete selected components
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

        //controller to add new data center 
        public void addDataCenterToDB(string pstrDataCenterName, string pstrDataCentertype)
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            performObj.AddingNewDataCenterToDB(pstrDataCenterName, pstrDataCentertype);
        }

        //controller to fill the data center lists
        public JsonResult fillDataCenterListOnPage(string pstrDataCenterType)
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            List<DBDataCenter> testList = new List<DBDataCenter>();
            testList = performObj.fillingDataCenterListOnPage(pstrDataCenterType);
            return Json(testList, JsonRequestBehavior.AllowGet);
        }

        //controller to populate the data center list in component page
        public JsonResult getCompanyDetailWithId(string pstrCompanyId) 
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            return Json(performObj.populatingDataCenterListOnComponentPage(pstrCompanyId), JsonRequestBehavior.AllowGet);
        }

        //controller to update the data Center in DB
        public void changeDataCenter(string pstrListOfCheckBoxes, string pstrCompanyId, string pstrDataCenterSelected) 
        {
            List<string> mListOfCheckBoxes = new List<string>();
            string[] separatedCheckBoxes = pstrListOfCheckBoxes.Split(new char[] { '&' });
            foreach (string separateComponent in separatedCheckBoxes)
            {
                string[] componentProperty = separateComponent.Split(new char[] { '=' });
                mListOfCheckBoxes.Add(componentProperty[1]);
            }
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            performObj.ChangingDataCenter(mListOfCheckBoxes, pstrCompanyId, pstrDataCenterSelected);
        }

        //controller to check if incident exists 
        public string checkIfIncidentExists(string pstrComponentId) 
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            return (performObj.checkingIfIncidentExists(pstrComponentId));
        }

        //controller to delete existing incident 
        public void deleteExistingIncidentOfComponent(string pstrComponentId, string pstrCompanyId) 
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            performObj.deletingExistingIncidentOfComponent(pstrComponentId, pstrCompanyId);
        }


        public void setPrimaryDataCenter(string pstrCompanyId, string pstrComponentsSelected, string pstrSelectedDataCenter) 
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();

            List<string> mListOfComponents = new List<string>();
            string strResult = string.Empty;
            string[] separatedCheckBoxes = pstrComponentsSelected.Split(new char[] { '&' });
            foreach (string separateComponent in separatedCheckBoxes)
            {
                string[] componentProperty = separateComponent.Split(new char[] { '=' });
                mListOfComponents.Add(componentProperty[1]);
            }

            performObj.settingPrimaryDataCenter(pstrCompanyId, mListOfComponents, pstrSelectedDataCenter); 
        }

        //controller to get incident Details based on componentId
        public JsonResult getIncidentDetails(string pstrComponentId) 
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            return Json(performObj.gettingIncidentDetails(pstrComponentId),JsonRequestBehavior.AllowGet);
        }
    }
}

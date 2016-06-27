using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WinMonitorApp.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Globalization;

namespace WinMonitorApp.Controllers
{
    public class AdminController : Controller
    {
        //Screen to login into administrator dashboard and create a session
        public ActionResult LoginScreen()
        {
            return View();
        }

        //ajax call to check if credentials are valid
        public string jsonCredentialsCheck(string pstrUsername, string pstrPassword)
        {
            PerformLogin LoginObj = new PerformLogin();
            string result = LoginObj.mCredentialCheck(pstrUsername, pstrPassword);
            return result;
        }


        //controller to display Master Page
        public ActionResult Master(string CheckLogin, string UserName)
        {
            if (CheckLogin == "CredentialTrue")
            {
                ViewBag.UserName = UserName;
                return View(ViewBag);
            }
            else
            {
                return View("LoginScreen");
            }
        }

        //controller to display Company Page
        public ActionResult CompanyDashboard(string commonCheck)
        {
            if (commonCheck == "CheckIfTrueErrorWin")
            {
            PerformCompanyComponentWithStatusIncident PerformCompanyComponentWithStatusIncidentObj = new PerformCompanyComponentWithStatusIncident();
            List<DBCompany> ModelCompany = PerformCompanyComponentWithStatusIncidentObj.mRetrieveCompanyDetails();
            return View(ModelCompany);
            }
            else
            {
                return null;
            }
        }


        //class for getCompanyDataCenter
        public class jsonCompanyCenter
        {
            public string GetCompanyId {get; set;}
        }


        //controller to display Component Page
        public ActionResult ComponentandStatus(string commonCheck, string companyId)
        {
            if ((commonCheck == "CheckIfTrueErrorWin") && (companyId != ""))
            {
                return View();
            }
            else
            {
                return null;
            }
        }



        //controllet to display performance metrics page
        public ActionResult PerformanceMetrics(string commonCheck, string companyId)
        {
            if ((commonCheck == "CheckIfTrueErrorWin") && (companyId != ""))
            {
                return View();
            }
            else
            {
                return null;
            }
        }



        //controller to display Editable MaintenanceCalendar page
        public ActionResult EditMaintenanceCalendar(string commonCheck, string companyId)
        {
            if ((commonCheck == "CheckIfTrueErrorWin") && (companyId != ""))
            {
                return View();
            }
            else
            {
                return null;
            }
        }

        //Day Click Event
        public string jsonAddDayClickEvents(string jsonpstrTitle, string jsonpstrDetails, string jsonpstrStartTime, string jsonpstrEndTime, string jsonpstrEventFor, string jsonpstrMaintenance, string jsonpstrCompanyId)
        {
            PerformMaintenanceCalendar performDayEventObj = new PerformMaintenanceCalendar();
            string resultString = performDayEventObj.mSaveCalendarEvent(jsonpstrTitle, jsonpstrDetails, jsonpstrStartTime, jsonpstrEndTime, jsonpstrEventFor, jsonpstrMaintenance, Int32.Parse(jsonpstrCompanyId));
            return resultString;
        }

        //Event ClickShow        --COMBINED 
        public JsonResult jsonEventClick(string jsonpstrgetEventId)
        {
            PerformMaintenanceCalendar performEventClickObj = new PerformMaintenanceCalendar();
            List<DummyCalendar> modelDetails = performEventClickObj.mGetEventCalendarDetails(Int32.Parse(jsonpstrgetEventId));                                                             // to be changed
            return Json(modelDetails, JsonRequestBehavior.AllowGet);
        }

        //Event ClickDelete      --COMBINED
        public string jsonEventClickDelete(string jsonpstrEventId)
        {
            PerformMaintenanceCalendar performEventDeleteObj = new PerformMaintenanceCalendar();
            string result = performEventDeleteObj.mDeleteEvent(Int32.Parse(jsonpstrEventId));
            return result;
        }

        //controller to inactivate Events
        public void InactivateEvents() 
        {
            PerformMaintenanceCalendar performObj = new PerformMaintenanceCalendar();
            performObj.InactivatingEvents();
        }

        //function to get existing events from db
        public JsonResult getExistingEvents(string pstrCompanyId)
        {
            
            PerformMaintenanceCalendar performObj = new PerformMaintenanceCalendar();
            var events = (performObj.gettingExistingEvents(Int32.Parse(pstrCompanyId))).ToArray();
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        //function to update event in DB
        public void UpdateCalenderInDB(string pstrEventId, DateTime pstrNewEventStart, DateTime pstrNewEventEnd)
        {
            PerformMaintenanceCalendar performObj = new PerformMaintenanceCalendar();
            performObj.UpdatingCalenderInDB(Int32.Parse(pstrEventId), pstrNewEventStart, pstrNewEventEnd);
        }

        //controller to display RegisteredAdmin page
        public ActionResult RegisteredAdmin(string commonCheck)
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

        public JsonResult jsonGetUser()
        {
            PerformLogin LoginUserObj =new PerformLogin();
            List<DBLogin> ModelLogin = LoginUserObj.mRetrieveLoginDetails();
            return Json(ModelLogin, JsonRequestBehavior.AllowGet);
            
        }


        //controller to display Settings page
        public ActionResult Settings(string commonCheck)
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

        public string jsonChangeSettings(string pstrUsername, string pstrOldPassword, string pstrNewPassword)
        {
            PerformLogin LoginSettingsObj = new PerformLogin();
            string resultSetting = LoginSettingsObj.mChangeSettings(pstrUsername, pstrOldPassword, pstrNewPassword);

            return resultSetting;
        }



        //controller to return component details based on company name to component page
        public JsonResult ComponentandStatusFromDB(string pstrCompanyIdFromPage)
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            IEnumerable<DummyClassForComponentPageDisplay> resultObj = performObj.LoadExistingComponentsFromDataBase(Int32.Parse(pstrCompanyIdFromPage));
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
            return (performObj.AddComponentToDb(MasterComponentListFromPage, Int32.Parse(pstrCompanyId), pstrPrimaryDataCenterName));

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
            public int jsonSpecificComponentCompanyId { get; set; } //comes from global variable
            public string jsonSpecificComponentDataCenter { get; set; }
        }

        public string jsonSpecificComponentRetrieve(jsonSpecificComponentServer jsonSpecificComponentServers)
        {
            string mSpecificComponentNameSend = jsonSpecificComponentServers.jsonSpecificComponentName.ToString();
            string mSpecificComponentStatusSend = jsonSpecificComponentServers.jsonSpecificComponentStatus.ToString();
            string mSpecificComponentCompanyId = jsonSpecificComponentServers.jsonSpecificComponentCompanyId.ToString();
            string mSpecificComponentDataCenter= jsonSpecificComponentServers.jsonSpecificComponentDataCenter.ToString();

            PerformCompanyComponentWithStatusIncident SpecificComponentAddObj = new PerformCompanyComponentWithStatusIncident();
            string catchExceptionSpecificComponent = SpecificComponentAddObj.mSaveAddSpecificComponentDetails(mSpecificComponentNameSend, mSpecificComponentStatusSend, Int32.Parse(mSpecificComponentCompanyId), mSpecificComponentDataCenter);

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
            int mCompanyId = Int32.Parse(jsonIncidentServers.jsonCompanyID);
            List<int> componentIdList = new List<int>();
            foreach (string componentId in mComponentIdList) 
            {
                componentIdList.Add(Int32.Parse(componentId));
            }

            PerformCompanyComponentWithStatusIncident IncidentAddObj = new PerformCompanyComponentWithStatusIncident();
            string catchExceptionIncident = IncidentAddObj.mSaveAddIncidentDetails(mIncidentNameSend, mIncidentDetailsSend, componentIdList, mCompanyId);

            return catchExceptionIncident;
        }


        //controller to update the status of selected components
        public string updateStatusOfSelectedComponents(string pstrListOfCheckBoxes, string pstrStatusSelected, string pstrCompanyId)
        {
            List<int> mListOfCheckBoxes = new List<int>();
            string strResult = string.Empty;
            string[] separatedCheckBoxes = pstrListOfCheckBoxes.Split(new char[] { '&' });
            foreach (string separateComponent in separatedCheckBoxes)
            {
                string[] componentProperty = separateComponent.Split(new char[] { '=' });
                mListOfCheckBoxes.Add(Int32.Parse(componentProperty[1]));
            }
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            strResult = performObj.UpdatingStatusOfComponentsSelected(mListOfCheckBoxes, pstrStatusSelected, Int32.Parse(pstrCompanyId));
            return (strResult);
        }

        //controller to delete selected components
        public void deleteSelectedComponents(string pComponentList, string pCompanyId)
        {
            List<int> mListOfComponents = new List<int>();
            string strResult = string.Empty;
            string[] separatedCheckBoxes = pComponentList.Split(new char[] { '&' });
            foreach (string separateComponent in separatedCheckBoxes)
            {
                string[] componentProperty = separateComponent.Split(new char[] { '=' });
                mListOfComponents.Add(Int32.Parse(componentProperty[1]));
            }
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            performObj.DeleteSelectedComponents(mListOfComponents, Int32.Parse(pCompanyId));

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
        public JsonResult getCompanyDetailWithId(string  pstrCompanyId) 
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            return Json(performObj.populatingDataCenterListOnComponentPage(Int32.Parse(pstrCompanyId)), JsonRequestBehavior.AllowGet);
        }

        //controller to update the data Center in DB
        public void changeDataCenter(string pstrListOfCheckBoxes, string pstrCompanyId, string pstrDataCenterSelected) 
        {
            List<int> mListOfCheckBoxes = new List<int>();
            string[] separatedCheckBoxes = pstrListOfCheckBoxes.Split(new char[] { '&' });
            foreach (string separateComponent in separatedCheckBoxes)
            {
                string[] componentProperty = separateComponent.Split(new char[] { '=' });
                mListOfCheckBoxes.Add(Int32.Parse(componentProperty[1]));
            }
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            performObj.ChangingDataCenter(mListOfCheckBoxes, Int32.Parse(pstrCompanyId), pstrDataCenterSelected);
        }

        //controller to check if incident exists 
        public string checkIfIncidentExists(string pstrComponentId) 
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            return (performObj.checkingIfIncidentExists(Int32.Parse(pstrComponentId)));
        }

        //controller to delete existing incident 
        public void deleteExistingIncidentOfComponent(string pstrComponentId, string pstrCompanyId) 
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            performObj.deletingExistingIncidentOfComponent(Int32.Parse(pstrComponentId), Int32.Parse(pstrCompanyId));
        }


        public void setPrimaryDataCenter(string pstrCompanyId, string pstrComponentsSelected, string pstrSelectedDataCenter) 
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();

            List<int> mListOfComponents = new List<int>();
            string strResult = string.Empty;
            string[] separatedCheckBoxes = pstrComponentsSelected.Split(new char[] { '&' });
            foreach (string separateComponent in separatedCheckBoxes)
            {
                string[] componentProperty = separateComponent.Split(new char[] { '=' });
                mListOfComponents.Add(Int32.Parse(componentProperty[1]));
            }

            performObj.settingPrimaryDataCenter(Int32.Parse(pstrCompanyId), mListOfComponents, pstrSelectedDataCenter); 
        }

        //controller to get incident Details based on componentId
        public JsonResult getIncidentDetails(string pstrComponentId) 
        {
            PerformCompanyComponentWithStatusIncident performObj = new PerformCompanyComponentWithStatusIncident();
            return Json(performObj.gettingIncidentDetails(Int32.Parse(pstrComponentId)),JsonRequestBehavior.AllowGet);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using WinMonitorApp.Models;
using System.Net;
using System.IO;

namespace WinMonitorApp.Models
{
    public class PerformCompanyComponentWithStatusIncident
    {

        // To generate sequences from the self generated sequences in the database
            //to generate company id from sequence
            public int getseqDBCompanyId()
            {
                WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                return mDBContext.Database.SqlQuery<int>("select next value for seqDBCompanyId").FirstOrDefault();
            }

            //to generate incident id from sequence
            public int getseqDBIncidentId()
            {
                WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                return mDBContext.Database.SqlQuery<int>("select next value for seqDBIncidentId").FirstOrDefault();
            }

            //to generate component id from sequence
            public int getseqDBCSId()
            {
                WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                return mDBContext.Database.SqlQuery<int>("select next value for seqSpecificDBCSId").FirstOrDefault();
            }

            //to generate Log id from sequence
            public int getseqDBLogId()
            {
                WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                return mDBContext.Database.SqlQuery<int>("select next value for seqDBLogId").FirstOrDefault();
            }

            //to generate data center id from sequence
            public int getseqDBDataCenterId()
            {
                WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                return mDBContext.Database.SqlQuery<int>("select next value for seqDBDataCenterId").FirstOrDefault();
            }


        //USER FUNCTIONS
            //get user CompanyId from CompanyName in url
            public int mGetUserCompanyId(string pstringCompanyName)
            {
                WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();

                int mCompanyId = mDBContext.Database.SqlQuery<int>("select DBCompanyId from DBCompany where DBCompanyName='" + pstringCompanyName + "'").FirstOrDefault();

                return mCompanyId;
            }

            //show status page values
            public List<DBComponent_With_Status> mGetStatus(int pstringCompanyStatusId)
            {
                WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                return mDBContext.Database.SqlQuery<DBComponent_With_Status>("select * from DBComponent_With_Status where DBCompanyId = '" + pstringCompanyStatusId + "';").ToList();
            }

            //show history page values
            public List<DummyHistory> mGetHistory(int pstringCompanyHistoryId)
            {
                WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                return mDBContext.Database.SqlQuery<DummyHistory>("select DBComponentName as ComponentName, DBIncidentName as IncidentName, DBIncidentDetails as IncidentDetails, DBDateTimeStart as GetDateTime from DBLogHistory where DBCompanyId=" + pstringCompanyHistoryId + " order by DBDateTimeStart desc;").ToList();
            }

            //get incident details of component with component id
            public DBLogHistory gettingIncidentDetailsOfComponent(int componentId)
            {
                WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                DBLogHistory historyObj = new DBLogHistory();
                historyObj = contextObj.Database.SqlQuery<DBLogHistory>("select * from DBLogHistory where DBCSId ='" + componentId + "';").Last();
                return historyObj;
            }

        //ADMIN FUNCTIONS       
            //Company management functions
                //show company details
                public List<DBCompany> mRetrieveCompanyDetails()
                {
                    WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                    return mDBContext.DBCompanies.ToList();
                }

                //getting existing components
                public List<DBCompany> loadingExistingCompanies()
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    List<DBCompany> companyList = new List<DBCompany>();
                    companyList = contextObj.Database.SqlQuery<DBCompany>("Select * from DBCompany").ToList();
                    return companyList;
                }

                //Company Save details method
                public string mSaveAddCompanyDetails(string pstringCompanyName, string pstringURL, string pstringPrimarySelect, string pstringSecondarySelect)
                {
                    try
                    {
                        WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                        DBCompany mDBCompanyObj = new DBCompany();


                        mDBCompanyObj.DBCompanyId = getseqDBCompanyId();
                        mDBCompanyObj.DBCompanyName = pstringCompanyName;
                        mDBCompanyObj.DBURL = pstringURL;
                        mDBCompanyObj.DBPrimaryCenter = pstringPrimarySelect;
                        mDBCompanyObj.DBSecondaryCenter = pstringSecondarySelect;

                        //adds new to the company
                        mDBContext.DBCompanies.Add(mDBCompanyObj);

                        //save the company details to the database
                        mDBContext.SaveChanges();
                        return "Company Successfully added !!";
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


                //fetching companyName from companyId method 
                public string GetCompanyNameById(int pstrCompanyId)
                {
                    WinMonitorEntityModelContext contextObj;
                    DBCompany result;
                    contextObj = new WinMonitorEntityModelContext();
                    result = (from company in contextObj.DBCompanies
                              where company.DBCompanyId == pstrCompanyId
                              select company).SingleOrDefault();
                    return result.DBCompanyName;
                }

                //fetching company Details by company id
                public DBCompany getCompanyDetailsById(int companyId)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    DBCompany companyObj = new DBCompany();
                    companyObj = contextObj.Database.SqlQuery<DBCompany>("select * from DBCompany where DBCompanyId = '" + companyId + "';").First();
                    return companyObj;
                }


            //Component management functions
                //Specific Components Save details method
                public string mSaveAddSpecificComponentDetails(string pstringSpecificComponentName, string pstringSpecificComponentStatus, int pstringSpecificComponentCompanyId, string pstringSpecificComponentDataCenter)
                {
                    try
                    {
                        WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                        DBComponent_With_Status mDBSpecific_Component_With_StatusObj = new DBComponent_With_Status();
                        DBLogHistory mDBLogObj = new DBLogHistory();

                        mDBSpecific_Component_With_StatusObj.DBCSId = getseqDBCSId();
                        mDBSpecific_Component_With_StatusObj.DBComponentName = pstringSpecificComponentName;
                        mDBSpecific_Component_With_StatusObj.DBStatus = pstringSpecificComponentStatus;
                        mDBSpecific_Component_With_StatusObj.DBType = "Specific";
                        mDBSpecific_Component_With_StatusObj.DBCompanyId = pstringSpecificComponentCompanyId;
                        mDBSpecific_Component_With_StatusObj.DBMasterComponentName = null;
                        mDBSpecific_Component_With_StatusObj.DBCenterName = pstringSpecificComponentDataCenter;


                        //adds new specific component to the components table
                        mDBContext.DBComponent_With_Status.Add(mDBSpecific_Component_With_StatusObj);

                        //save the specific component details to the database
                        mDBContext.SaveChanges();


                        mDBLogObj.DBLogId = getseqDBLogId();
                        mDBLogObj.DBCompanyId = pstringSpecificComponentCompanyId;
                        mDBLogObj.DBCSId = mDBSpecific_Component_With_StatusObj.DBCSId;
                        mDBLogObj.DBIncidentId = -1;
                        mDBLogObj.DBComponentName = mDBSpecific_Component_With_StatusObj.DBComponentName;
                        mDBLogObj.DBIncidentName = "";
                        mDBLogObj.DBIncidentDetails = "";
                        mDBLogObj.DBDateTimeStart = DateTime.UtcNow; ;
                        mDBLogObj.DBStatus = pstringSpecificComponentStatus;
                        mDBLogObj.DBDateTimeEnd = null;

                        //adds component to log history
                        mDBContext.DBLogHistories.Add(mDBLogObj);

                        //save the log details to the database
                        mDBContext.SaveChanges();

                        return "Specific Component Successfully added !!";
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
        
                //Display Component Page Elements
                public IEnumerable<DummyClassForComponentPageDisplay> LoadExistingComponentsFromDataBase(int pCompanyId)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    IEnumerable<DummyClassForComponentPageDisplay> result;
                    result = contextObj.Database.SqlQuery<DummyClassForComponentPageDisplay>("select cws.DBCSId as mstrComponentId, cws.DBComponentName as mstrComponentName, cws.DBType as mstrComponentType, cws.DBStatus as mstrComponentStatus, comp.DBCompanyName as mstrCompanyName, i.DBIncidentName as mstrIncidentName, cws.DBCenterName as mstrCenterName from DBComponent_With_Status cws inner join DBCompany comp on cws.DBCompanyId = comp.DBCompanyId left join DBIncidents i on i.DBCSId = cws.DBCSId where comp.DBCompanyId= " + pCompanyId + "").ToList();
                    return result;
                }
        
                //fetching master component list from database
                public IEnumerable<DBMaster_DBComponent_With_Status> MasterComponentDataFromDB()
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    IEnumerable<DBMaster_DBComponent_With_Status> masterComponentList;
                    masterComponentList = contextObj.Database.SqlQuery<DBMaster_DBComponent_With_Status>("select * from dbo.DBMaster_DBComponent_With_Status").AsEnumerable();
                    return masterComponentList;
                }
        
                //Method to add master component to component list of company in database 
                public string AddComponentToDb(List<string> pMasterComponentListFromPage, int pCompanyId, string pPrimaryDataCenter)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();

                    try
                    {
                        foreach (string masterComponentName in pMasterComponentListFromPage)
                        {
                            DBComponent_With_Status component = new DBComponent_With_Status();
                            DBLogHistory historyObj = new DBLogHistory();
                            component.DBCSId = getseqDBCSId();
                            component.DBComponentName = masterComponentName;
                            component.DBStatus = "Operational";
                            component.DBType = "master";
                            component.DBCompanyId = pCompanyId;
                            component.DBMasterComponentName = masterComponentName;
                            component.DBCenterName = pPrimaryDataCenter;

                            //adding master component to db
                            contextObj.DBComponent_With_Status.Add(component);

                            contextObj.SaveChanges();

                            //code to add details to log hisory 
                            historyObj.DBLogId = getseqDBLogId();
                            historyObj.DBCompanyId = pCompanyId;
                            historyObj.DBCSId = component.DBCSId;
                            historyObj.DBIncidentId = -1;
                            historyObj.DBComponentName = component.DBComponentName;
                            historyObj.DBIncidentName = "";
                            historyObj.DBIncidentDetails = "";
                            historyObj.DBDateTimeStart = DateTime.UtcNow; ;
                            historyObj.DBStatus = component.DBStatus;
                            historyObj.DBDateTimeEnd = null;

                            //adds history obj to database
                            contextObj.DBLogHistories.Add(historyObj);

                            contextObj.SaveChanges();
                        }

                        return "Master Component added added !!";

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

                }
        
                //method to get all components related to a company
                public List<DBComponent_With_Status> gettingComponents(int pstrCompanyId)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    List<DBComponent_With_Status> listOfComponents = new List<DBComponent_With_Status>();
                    listOfComponents = contextObj.Database.SqlQuery<DBComponent_With_Status>("select * from DBComponent_With_Status where DBCompanyId = " + pstrCompanyId + ";").ToList();
                    return listOfComponents;

                }

                //method to delete selected components
                public void DeleteSelectedComponents(List<int> pListOfComponents, int pCompanyId)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    foreach (int componentId in pListOfComponents)
                    {
                        DBIncident incidentObj = new DBIncident();
                        DBLogHistory historyObj = new DBLogHistory();
                        DBComponent_With_Status componentObj = new DBComponent_With_Status();
                        int incidentIdStoringVariable = -1;

                        incidentObj = (from incident in contextObj.DBIncidents
                                       where incident.DBCSId == componentId
                                       select incident).FirstOrDefault();

                        componentObj = (from component in contextObj.DBComponent_With_Status
                                        where component.DBCSId == componentId
                                        select component).FirstOrDefault();

                        if (incidentObj != null)
                        {
                            incidentIdStoringVariable = incidentObj.DBIncidentId;

                            //Adding log history
                            historyObj.DBCompanyId = pCompanyId;
                            historyObj.DBComponentName = componentObj.DBComponentName;
                            historyObj.DBCSId = componentObj.DBCSId;
                            historyObj.DBDateTimeStart = DateTime.UtcNow; ;
                            historyObj.DBIncidentId = incidentIdStoringVariable;
                            historyObj.DBDateTimeEnd = null;
                            historyObj.DBIncidentDetails = "Incident Deleted";
                            historyObj.DBIncidentName = incidentObj.DBIncidentName;
                            historyObj.DBLogId = getseqDBLogId();
                            historyObj.DBStatus = "Operational";
                            contextObj.DBLogHistories.Add(historyObj);
                            contextObj.SaveChanges();

                            contextObj.DBIncidents.Remove(incidentObj);
                            contextObj.SaveChanges();
                        }
                        else
                        {
                            historyObj = new DBLogHistory();
                            //Adding log history
                            historyObj.DBCompanyId = pCompanyId;
                            historyObj.DBComponentName = componentObj.DBComponentName;
                            historyObj.DBCSId = componentObj.DBCSId;
                            historyObj.DBDateTimeStart = DateTime.UtcNow; ;
                            historyObj.DBDateTimeEnd = null;
                            historyObj.DBIncidentId = incidentIdStoringVariable;
                            historyObj.DBIncidentDetails = "Component Deleted";
                            historyObj.DBIncidentName = "Component Deleted";
                            historyObj.DBLogId = getseqDBLogId();
                            historyObj.DBStatus = "Operational";
                            contextObj.DBLogHistories.Add(historyObj);
                            contextObj.SaveChanges();
                        }
                        contextObj.DBComponent_With_Status.Remove(componentObj);
                        contextObj.SaveChanges();
                    }

                }

                //update staus of selected components
                public string UpdatingStatusOfComponentsSelected(List<int> pselectedComponentList, string pStatusToBeChanged, int pCompanyId)
                {
                    try
                    {
                        WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                        DBLogHistory historyObj = new DBLogHistory();
                        DBCompany companyObj = new DBCompany();
                        string previousStatus;
                        companyObj = (from company in contextObj.DBCompanies
                                      where company.DBCompanyId == pCompanyId
                                      select company).FirstOrDefault();
                        string strResult = String.Empty;
                        if (pStatusToBeChanged == "Operational")
                        {
                            foreach (int componentId in pselectedComponentList)
                            {
                                DBComponent_With_Status componentObj = new DBComponent_With_Status();
                                DBIncident incidentObj = new DBIncident();

                                componentObj = (from component in contextObj.DBComponent_With_Status
                                                where component.DBCSId == componentId
                                                select component).FirstOrDefault();
                                if (companyObj.DBSecondaryCenter != componentObj.DBCenterName)
                                {
                                    incidentObj = (from incident in contextObj.DBIncidents
                                                   where incident.DBCSId == componentId
                                                   select incident).FirstOrDefault();
                                    if (incidentObj != null)
                                    {
                                        historyObj = (from histObj in contextObj.DBLogHistories
                                                      where ((histObj.DBCompanyId == pCompanyId) && (histObj.DBCSId == componentId) && (histObj.DBIncidentId == incidentObj.DBIncidentId))
                                                      orderby histObj.DBDateTimeStart descending, histObj.DBDateTimeEnd descending
                                                      select histObj).First();
                                        historyObj.DBDateTimeEnd = DateTime.UtcNow; ;
                                        contextObj.SaveChanges();
                                        contextObj.DBIncidents.Remove(incidentObj);
                                        contextObj.SaveChanges();
                                    }
                                    componentObj.DBStatus = pStatusToBeChanged;
                                    contextObj.SaveChanges();

                                }
                                List<DBSubscription> subscriptionObjList = new List<DBSubscription>();
                                subscriptionObjList = (from subObj in contextObj.DBSubscriptions
                                                       where subObj.DBCompanyId == pCompanyId
                                                       select subObj).ToList();
                                DBEmailPage emailPageObj = new DBEmailPage();
                                emailPageObj = (from emailPage in contextObj.DBEmailPages
                                                where emailPage.DBEmailPageId == 1
                                                select emailPage).FirstOrDefault();

                                foreach (DBSubscription subscriptionObj in subscriptionObjList)
                                {
                                    PerformSubscription performSubscriptionObj = new PerformSubscription();
                                    WebClient clientObj = new WebClient();
                                    int endIndex, startIndex;
                                    string stringToBeReplaced;
                                    string stringHtml = emailPageObj.DBEmailContent.ToString();

                                    endIndex = stringHtml.IndexOf("<!--end of eventsUpdateDiv-->");
                                    startIndex = stringHtml.IndexOf("<div id=\"divForEvents\">");
                                    stringToBeReplaced = stringHtml.Substring(startIndex, endIndex - startIndex);
                                    stringHtml = stringHtml.Replace(stringToBeReplaced, "");

                                    endIndex = stringHtml.IndexOf("<!--end of dataCenterChangeUpdate-->");
                                    startIndex = stringHtml.IndexOf("<div id=\"divForDataCenterUpdate\">");
                                    stringToBeReplaced = stringHtml.Substring(startIndex, endIndex - startIndex);
                                    stringHtml = stringHtml.Replace(stringToBeReplaced, "");

                                    string link = "http://cha-en-pdp2:2108/?pUserCompanyName=CompanyNameVariable";                              //change url to direct to user page
                                    stringHtml = stringHtml.Replace("linkToBeChanged", link);
                                    stringHtml = stringHtml.Replace("CompanyNameVariable", companyObj.DBCompanyName);
                                    stringHtml = stringHtml.Replace("customerNameVariable", subscriptionObj.DBName);
                                    stringHtml = stringHtml.Replace("ComponentNameForStatusUpdateVariable", componentObj.DBComponentName);
                                    stringHtml = stringHtml.Replace("ComponentStatusUpdateVariable", componentObj.DBStatus);
                                    performSubscriptionObj.sendEmail(subscriptionObj.DBEmail, null, "Component Status Updated", stringHtml);
                                }

                            }
                            strResult = "Incident Successfully deleted !!";
                        }
                        else
                        {
                            foreach (int componentId in pselectedComponentList)
                            {
                                DBComponent_With_Status componentObj = new DBComponent_With_Status();

                                componentObj = (from component in contextObj.DBComponent_With_Status
                                                where component.DBCSId == componentId
                                                select component).FirstOrDefault();
                                previousStatus = componentObj.DBStatus;
                                componentObj.DBStatus = pStatusToBeChanged;
                                contextObj.SaveChanges();
                                DBIncident incidentObj = new DBIncident();
                                incidentObj = (from incident in contextObj.DBIncidents
                                               where incident.DBCSId == componentId
                                               select incident).FirstOrDefault();
                                //code to add details to log hisory 
                                historyObj.DBLogId = getseqDBLogId();
                                historyObj.DBCompanyId = pCompanyId;
                                historyObj.DBCSId = componentId;
                                historyObj.DBIncidentId = incidentObj.DBIncidentId;
                                historyObj.DBComponentName = componentObj.DBComponentName;
                                historyObj.DBIncidentName = incidentObj.DBIncidentName;
                                historyObj.DBIncidentDetails = incidentObj.DBDescription;
                                historyObj.DBDateTimeStart = DateTime.UtcNow; ;
                                historyObj.DBStatus = componentObj.DBStatus;
                                historyObj.DBDateTimeEnd = null;

                                List<DBSubscription> subscriptionObjList = new List<DBSubscription>();
                                subscriptionObjList = (from subObj in contextObj.DBSubscriptions
                                                       where subObj.DBCompanyId == pCompanyId
                                                       select subObj).ToList();
                                DBEmailPage emailPageObj = new DBEmailPage();
                                emailPageObj = (from emailPage in contextObj.DBEmailPages
                                                where emailPage.DBEmailPageId == 1
                                                select emailPage).FirstOrDefault();

                                foreach (DBSubscription subscriptionObj in subscriptionObjList)
                                {
                                    PerformSubscription performSubscriptionObj = new PerformSubscription();
                                    WebClient clientObj = new WebClient();
                                    int endIndex, startIndex;
                                    string stringToBeReplaced;
                                    string stringHtml = emailPageObj.DBEmailContent.ToString();

                                    endIndex = stringHtml.IndexOf("<!--end of eventsUpdateDiv-->");
                                    startIndex = stringHtml.IndexOf("<div id=\"divForEvents\">");
                                    stringToBeReplaced = stringHtml.Substring(startIndex, endIndex - startIndex);
                                    stringHtml = stringHtml.Replace(stringToBeReplaced, "");

                                    endIndex = stringHtml.IndexOf("<!--end of dataCenterChangeUpdate-->");
                                    startIndex = stringHtml.IndexOf("<div id=\"divForDataCenterUpdate\">");
                                    stringToBeReplaced = stringHtml.Substring(startIndex, endIndex - startIndex);
                                    stringHtml = stringHtml.Replace(stringToBeReplaced, "");

                                    string link = "http://cha-en-pdp2:2108/?pUserCompanyName=CompanyNameVariable";                              //change url to direct to user page
                                    stringHtml = stringHtml.Replace("linkToBeChanged", link);
                                    stringHtml = stringHtml.Replace("CompanyNameVariable", companyObj.DBCompanyName);
                                    stringHtml = stringHtml.Replace("customerNameVariable", subscriptionObj.DBName);
                                    stringHtml = stringHtml.Replace("ComponentNameForStatusUpdateVariable", componentObj.DBComponentName);
                                    stringHtml = stringHtml.Replace("ComponentStatusUpdateVariable", componentObj.DBStatus);
                                    performSubscriptionObj.sendEmail(subscriptionObj.DBEmail, null, "Component Status Updated", stringHtml);
                                }

                            }
                            strResult = "Incident Successfully added !!";
                        }
                        return strResult;
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

        
            //Incident Management functions
                //Incidents Save details method
                public string mSaveAddIncidentDetails(string pstringIncidentName, string pstringIncidentDetails, List<int> pstringComponentIdList, int pCompanyId)
                {
                    try
                    {
                        WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                        DBIncident mDBIncidentObj = new DBIncident();
                        DBLogHistory historyObj = new DBLogHistory();
                        foreach (int componentId in pstringComponentIdList)
                        {

                            mDBIncidentObj.DBIncidentId = getseqDBIncidentId();
                            mDBIncidentObj.DBIncidentName = pstringIncidentName;
                            mDBIncidentObj.DBDescription = pstringIncidentDetails;
                            mDBIncidentObj.DBCSId = componentId;


                            //adds new incident to the incidents table
                            mDBContext.DBIncidents.Add(mDBIncidentObj);

                            mDBContext.SaveChanges();

                            //code to add details to log hisory 
                            historyObj.DBLogId = getseqDBLogId();
                            historyObj.DBCompanyId = pCompanyId;
                            historyObj.DBCSId = componentId;
                            historyObj.DBIncidentId = mDBIncidentObj.DBIncidentId;
                            historyObj.DBComponentName = mDBContext.Database.SqlQuery<string>("select DBComponentName from DBComponent_With_Status where DBCSId=" + componentId + ";").FirstOrDefault();
                            historyObj.DBIncidentName = mDBIncidentObj.DBIncidentName;
                            historyObj.DBIncidentDetails = mDBIncidentObj.DBDescription;
                            historyObj.DBDateTimeStart = DateTime.UtcNow; ;
                            historyObj.DBStatus = mDBContext.Database.SqlQuery<string>("select DBStatus from DBComponent_With_Status where DBCSId=" + componentId + ";").FirstOrDefault();
                            historyObj.DBDateTimeEnd = null;

                            //adds history obj to database
                            mDBContext.DBLogHistories.Add(historyObj);

                            //save the specific component details to the database
                            mDBContext.SaveChanges();
                        }
                        return "Incident Successfully added !!";
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

                //method to check if incident exists corresponding to particular component
                public string checkingIfIncidentExists(int pstrComponentId)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    string ExistsOrNot;
                    int numberOfIncidents = (from incidentObj in contextObj.DBIncidents
                                             where incidentObj.DBCSId == pstrComponentId
                                             select incidentObj).Count();

                    if (numberOfIncidents == 0)
                        ExistsOrNot = "false";
                    else
                        ExistsOrNot = "true";

                    return ExistsOrNot;
                }

                //method to delete existing incident of particular element
                public void deletingExistingIncidentOfComponent(int pstrComponentId, int pstrCompanyId)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    DBIncident incidentObj = new DBIncident();
                    DBComponent_With_Status componentObj = new DBComponent_With_Status();
                    DBLogHistory historyObj = new DBLogHistory();

                    componentObj = (from component in contextObj.DBComponent_With_Status
                                    where component.DBCSId == pstrComponentId
                                    select component).FirstOrDefault();

                    incidentObj = (from incident in contextObj.DBIncidents
                                   where incident.DBCSId == pstrComponentId
                                   select incident).FirstOrDefault();
                    historyObj = (from log in contextObj.DBLogHistories
                                  where ((log.DBCompanyId == pstrCompanyId) && (log.DBCSId == pstrComponentId) && (log.DBDateTimeEnd == null))
                                  select log).Last();
                    historyObj.DBDateTimeStart = DateTime.UtcNow; ;
                    contextObj.SaveChanges();

                    contextObj.DBIncidents.Remove(incidentObj);
                    contextObj.SaveChanges();
                }

                //method to fetch incidentDetails from database
                public DBIncident gettingIncidentDetails(int pstrComponentId)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    DBIncident incidentObj = new DBIncident();
                    try
                    {

                        incidentObj = contextObj.Database.SqlQuery<DBIncident>("select * from DBIncidents where DBCSId=" + pstrComponentId + ";").FirstOrDefault();

                    }
                    catch (DbUpdateException exUpdateDB)
                    {
                        Console.Write(exUpdateDB);
                    }
                    catch (DbEntityValidationException exEntityValidateDB)
                    {
                        Console.Write(exEntityValidateDB);
                    }
                    catch (NotSupportedException exNotSupportedDB)
                    {
                        Console.Write(exNotSupportedDB);
                    }
                    catch (ObjectDisposedException exObjectDisposedDB)
                    {
                        Console.Write(exObjectDisposedDB);
                    }
                    catch (InvalidOperationException exInvalidOperationDB)
                    {
                        Console.Write(exInvalidOperationDB);
                    }

                    return incidentObj;
                }

                //function to get incident details from log history 
                public DBLogHistory gettingIncidentDetailsFromLog(int incidentId)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    DBLogHistory historyObj = new DBLogHistory();
                    historyObj = contextObj.Database.SqlQuery<DBLogHistory>("select * from DBLogHistory where DBIncidentId ='" + incidentId + "';").First();
                    return historyObj;
                }

                //function to update incident details 
                public void updatingIncidentDetails(int componentId, int expectedDuration)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    int n = contextObj.Database.ExecuteSqlCommand("update DBIncidents set DBExpectedDuration='" + expectedDuration + "' where DBCSID= '" + componentId + "';");

                }



            //Data center functions
                //method to add New Data Center
                public void AddingNewDataCenterToDB(string pstrDataCenterName, string pstrDataCenterType)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    DBDataCenter dataCenterObj = new DBDataCenter();
                    dataCenterObj.DBDataCenterId = getseqDBDataCenterId();
                    dataCenterObj.DBDataCenterName = pstrDataCenterName;
                    dataCenterObj.DBTypeName = pstrDataCenterType;
                    contextObj.DBDataCenters.Add(dataCenterObj);
                    contextObj.SaveChanges();

                }

                //method to return data center list
                public List<DBDataCenter> fillingDataCenterListOnPage(string pstrDataCenterType)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    List<DBDataCenter> dataCenterList = new List<DBDataCenter>();
                    dataCenterList = contextObj.Database.SqlQuery<DBDataCenter>("select * from DBDataCenter where DBTypeName='" + pstrDataCenterType + "';").ToList();
                    return (dataCenterList);
                }

                //method to populate data center list on component page
                public DBCompany populatingDataCenterListOnComponentPage(int pstrCompanyId)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    DBCompany companyObj = new DBCompany();
                    companyObj = contextObj.Database.SqlQuery<DBCompany>("select * from DBCompany where DBCompanyId=" + pstrCompanyId + ";").FirstOrDefault();
                    return companyObj;
                }

                //method to change data center of components list selected
                public void ChangingDataCenter(List<int> mListOfCheckBoxes, int pstrCompanyId, string pstrDataCenterSelected)
                {
                    WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                    DBCompany companyObj = new DBCompany();
                    companyObj = (from company in contextObj.DBCompanies
                                  where company.DBCompanyId == pstrCompanyId
                                  select company).FirstOrDefault();
                    foreach (int componentId in mListOfCheckBoxes)
                    {
                        DBComponent_With_Status componentObj = new DBComponent_With_Status();
                        componentObj = (from component in contextObj.DBComponent_With_Status
                                        where component.DBCSId == componentId
                                        select component).FirstOrDefault();
                        componentObj.DBCenterName = pstrDataCenterSelected;
                        contextObj.SaveChanges();
                        List<DBSubscription> subscriptionObjList = new List<DBSubscription>();
                        subscriptionObjList = (from subObj in contextObj.DBSubscriptions
                                               where subObj.DBCompanyId == pstrCompanyId
                                               select subObj).ToList();
                        DBEmailPage emailPageObj = new DBEmailPage();
                        emailPageObj = (from emailPage in contextObj.DBEmailPages
                                        where emailPage.DBEmailPageId == 1
                                        select emailPage).FirstOrDefault();

                        foreach (DBSubscription subscriptionObj in subscriptionObjList)
                        {
                            PerformSubscription performSubscriptionObj = new PerformSubscription();
                            WebClient clientObj = new WebClient();
                            int endIndex, startIndex;
                            string stringToBeReplaced;

                            string stringHtml = emailPageObj.DBEmailContent.ToString();

                            endIndex = stringHtml.IndexOf("<!--end of eventsUpdateDiv-->");
                            startIndex = stringHtml.IndexOf("<div id=\"divForEvents\">");
                            stringToBeReplaced = stringHtml.Substring(startIndex, endIndex - startIndex);
                            stringHtml = stringHtml.Replace(stringToBeReplaced, "");

                            endIndex = stringHtml.IndexOf("<!--end of componentStatusChangeDiv-->");
                            startIndex = stringHtml.IndexOf("<div id=\"divForComponentStatusUpdate\">");
                            stringToBeReplaced = stringHtml.Substring(startIndex, endIndex - startIndex);
                            stringHtml = stringHtml.Replace(stringToBeReplaced, "");

                            string link = "http://cha-en-pdp2:2108/?pUserCompanyName=CompanyNameVariable";                              //change url to direct to user page
                            stringHtml = stringHtml.Replace("linkToBeChanged", link);
                            stringHtml = stringHtml.Replace("CompanyNameVariable", companyObj.DBCompanyName);
                            stringHtml = stringHtml.Replace("customerNameVariable", subscriptionObj.DBName);
                            stringHtml = stringHtml.Replace("ComponentNameForDataCenterUpdateVariable", componentObj.DBComponentName);
                            stringHtml = stringHtml.Replace("DataCenterVariable", componentObj.DBCenterName);

                            performSubscriptionObj.sendEmail(subscriptionObj.DBEmail, null, "Data Center Updated", stringHtml);
                        }
                    }
                }
        
                //method to set data center to primary
                public void settingPrimaryDataCenter(int pstrCompanyId, List<int> mListOfComponents, string pstrSelectedDataCenter)
            {
                WinMonitorEntityModelContext contextObj = new WinMonitorEntityModelContext();
                DBCompany companyObj = new DBCompany();
                companyObj = (from company in contextObj.DBCompanies
                              where company.DBCompanyId == pstrCompanyId
                              select company).FirstOrDefault();
                foreach (int componentId in mListOfComponents)
                {
                    DBComponent_With_Status componentObj = new DBComponent_With_Status();
                    componentObj = (from component in contextObj.DBComponent_With_Status
                                    where component.DBCSId == componentId
                                    select component).FirstOrDefault();
                    componentObj.DBCenterName = pstrSelectedDataCenter;
                    contextObj.SaveChanges();
                    List<DBSubscription> subscriptionObjList = new List<DBSubscription>();
                    subscriptionObjList = (from subObj in contextObj.DBSubscriptions
                                           where subObj.DBCompanyId == pstrCompanyId
                                           select subObj).ToList();
                    DBEmailPage emailPageObj = new DBEmailPage();
                    emailPageObj = (from emailPage in contextObj.DBEmailPages
                                    where emailPage.DBEmailPageId == 1
                                    select emailPage).FirstOrDefault();

                    foreach (DBSubscription subscriptionObj in subscriptionObjList)
                    {
                        PerformSubscription performSubscriptionObj = new PerformSubscription();
                        WebClient clientObj = new WebClient();
                        int endIndex, startIndex;
                        string stringToBeReplaced;
                        string stringHtml = emailPageObj.DBEmailContent.ToString();
                        endIndex = stringHtml.IndexOf("<!--end of eventsUpdateDiv-->");
                        startIndex = stringHtml.IndexOf("<div id=\"divForEvents\">");
                        stringToBeReplaced = stringHtml.Substring(startIndex, endIndex - startIndex);
                        stringHtml = stringHtml.Replace(stringToBeReplaced, "");

                        endIndex = stringHtml.IndexOf("<!--end of componentStatusChangeDiv-->");
                        startIndex = stringHtml.IndexOf("<div id=\"divForComponentStatusUpdate\">");
                        stringToBeReplaced = stringHtml.Substring(startIndex, endIndex - startIndex);
                        stringHtml = stringHtml.Replace(stringToBeReplaced, "");

                        string link = "http://cha-en-pdp2:2108/?pUserCompanyName=CompanyNameVariable";                              //change url to direct to user page
                        stringHtml = stringHtml.Replace("linkToBeChanged", link);
                        stringHtml = stringHtml.Replace("CompanyNameVariable", companyObj.DBCompanyName);
                        stringHtml = stringHtml.Replace("customerNameVariable", subscriptionObj.DBName);
                        stringHtml = stringHtml.Replace("ComponentNameForDataCenterUpdateVariable", componentObj.DBComponentName);
                        stringHtml = stringHtml.Replace("DataCenterVariable", componentObj.DBCenterName);
                        performSubscriptionObj.sendEmail(subscriptionObj.DBEmail, null, "Data Center Updated", stringHtml);
                    }
                }

            }
        
    }
}
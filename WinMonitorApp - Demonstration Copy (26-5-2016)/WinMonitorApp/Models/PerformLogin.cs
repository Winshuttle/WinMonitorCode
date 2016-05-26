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
    public class PerformLogin
    {

        //Performing Screen Login
        public string mCredentialCheck(string sendUsername, string sendPassword)
        {
            try
            {
                WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();

                int checkObj = mDBContext.Database.SqlQuery<int>("select count(*) from DBLogin where DBUsername='" + sendUsername + "' and DBPassword='" + sendPassword + "' and DBAccountType ='Edit';").FirstOrDefault();
                if (checkObj == 1)
                {
                    return "true";
                }
                else
                {
                    return "false";
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


        // For Displaying List of Registered Administrators
        public List<DBLogin> mRetrieveLoginDetails()
        {
            WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
            return mDBContext.DBLogins.ToList();
        }


        //method for changing the password for the Administrator
        public string mChangeSettings(string sendUsername, string sendOldPassword, string sendNewPassword)
        {
            try
            {
                WinMonitorEntityModelContext mDBContext = new WinMonitorEntityModelContext();
                int checkIfExistingPasswordTrue = mDBContext.Database.SqlQuery<int>("select count(*) from DBLogin where DBUsername='" + sendUsername + "' and DBPassword='" + sendOldPassword + "';").FirstOrDefault();
                if (checkIfExistingPasswordTrue == 1)
                {
                    mDBContext.Database.ExecuteSqlCommand("update DBLogin set DBPassword = '" + sendNewPassword + "' where DBUsername = '" + sendUsername + "'; ");
                    return "Password Sucessfully Changed";
                }
                else
                {
                    return "Old Password Entered is incorrect";
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


    }
}
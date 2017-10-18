using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.UserAccessLog;
using SILDMS.Model.SecurityModule;
using System.Data.Common;

namespace SILDMS.DataAccess.UserAccessLog
{
    public class UserAccessLogDataService : IUserAccessLogDataService
    {
        #region Fields

        private readonly string spErrorParam = "@p_Error";
        //private readonly string spErrorParam = "@p_Status";

        #endregion

        #region Functions
        public List<SEC_UserLog> GetUserLog(string userID, DateTime? dateFrom, DateTime? dateTo, out string errorNumber)
        {
            errorNumber = string.Empty;
            var userLogins = new List<SEC_UserLog>();
            var factory = new DatabaseProviderFactory();
            var db = factory.CreateDefault() as SqlDatabase;
            using (var dbCommandWrapper = db.GetStoredProcCommand("GetUserLog"))
            {
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, userID);
                db.AddInParameter(dbCommandWrapper, "@DateFrom", SqlDbType.DateTime, dateFrom);
                db.AddInParameter(dbCommandWrapper, "@DateTo", SqlDbType.DateTime, dateTo);
                db.AddOutParameter(dbCommandWrapper, spErrorParam, SqlDbType.Int, 10);
                
                var ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, "@p_Error").IsNullOrZero())
                {
                    // Get the error number, if error occurred.
                    errorNumber = db.GetParameterValue(dbCommandWrapper, "@p_Error").PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count <= 0) return userLogins;
                    var dt = ds.Tables[0];
                    userLogins = dt.AsEnumerable().Select(ob => new SEC_UserLog
                    {
                        LogID = ob.GetString("LogID"),
                        UserID = ob.GetString("UserID"),
                        UserFullName = ob.GetString("UserFullName"),
                        UsedIP = ob.GetString("UsedIP"),
                        ActionUrl = ob.GetString("ActionUrl"),
                        ActionEventTime = string.Format("{0:dd/mm/yyyy}", ob.GetDateTime("ActionEventTime").ToString(CultureInfo.InvariantCulture)),
                        Status = ob.GetInt16("Status")
                    }).ToList();
                }
            }
            return userLogins;
        }

        public string ManipulateUserAccessLog(SEC_UserLog userLogin)
        {
            string errorNumber = String.Empty;
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;
                using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("Set_UserActionLogg"))
                {
                    // Set parameters 
                    db.AddInParameter(dbCommandWrapper, "@LogID", SqlDbType.NVarChar, userLogin.LogID);
                    db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, userLogin.UserID);
                    db.AddInParameter(dbCommandWrapper, "@UsedIP", SqlDbType.NVarChar, userLogin.UsedIP);
                    db.AddInParameter(dbCommandWrapper, "@UserAction ", SqlDbType.NVarChar, userLogin.UserAction);
                    db.AddInParameter(dbCommandWrapper, "@ActionUrl", SqlDbType.NVarChar, userLogin.ActionUrl);
                    db.AddInParameter(dbCommandWrapper, "@ActionEventTime", SqlDbType.DateTime, userLogin.ActionEventTime);
                    db.AddInParameter(dbCommandWrapper, "@ActionExecuteTime ", SqlDbType.NVarChar, userLogin.ActionExecuteTime);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, userLogin.Status);             
                    // Execute SP.
                    db.ExecuteNonQuery(dbCommandWrapper);
                    // Getting output parameters and setting response details.
                    errorNumber = "done";
                }
            }
            catch (Exception ex)
            {
                errorNumber = "faild"; // Log ex.Message  Insert Log Table               
            }
            return errorNumber;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.UserLevel;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccess.UserLevel
{
    public class UserLevelDataService : IUserLevelDataService
    {
        #region Fields

        private readonly string spErrorParam = "@p_Error";
        private readonly string spStatusParam = "@p_Status";

        #endregion

        #region Functions

        public List<SEC_UserLevel> GetUserLevel(int? userLevel, string action, string levelType, out string errorNumber)
        {
            errorNumber = string.Empty;
            var userLevelList = new List<SEC_UserLevel>();
            var factory = new DatabaseProviderFactory();
            var db = factory.CreateDefault() as SqlDatabase;
            using (var dbCommandWrapper = db.GetStoredProcCommand("GetUserLevel"))
            {
                db.AddInParameter(dbCommandWrapper, "@UserLevel", SqlDbType.VarChar, userLevel);
                db.AddInParameter(dbCommandWrapper, "@UserLevelType", SqlDbType.VarChar, levelType);
                db.AddOutParameter(dbCommandWrapper, spErrorParam, SqlDbType.Int, 10);
                var ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, "@p_Error").IsNullOrZero())
                {
                    // Get the error number, if error occurred.
                    errorNumber = db.GetParameterValue(dbCommandWrapper, "@p_Error").PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count <= 0) return userLevelList;
                    var dt = ds.Tables[0];
                    userLevelList = dt.AsEnumerable().Select(ob => new SEC_UserLevel
                    {
                        ID = ob.GetString("ID"),
                        UserLevel = ob.GetInt32("UserLevel"),
                        UserLevelName = ob.GetString("UserLevelName"),
                        UserLevelSL = ob.GetString("UserLevelSL"),
                        UserLevelType = ob.GetString("UserLevelType"),
                        SetOn = string.Format("{0:dd/mm/yyyy}", Convert.ToString(ob.GetDateTime("SetOn"), CultureInfo.InvariantCulture)),
                        SetBy = ob.GetString("SetBy"),
                        ModifiedOn = string.Format("{0:dd/mm/yyyy}", ob.GetDateTime("ModifiedOn").ToString(CultureInfo.InvariantCulture)),
                        ModifiedBy = ob.GetString("ModifiedBy"),
                        Status = ob.GetInt16("Status")
                    }).ToList();
                }
            }
            return userLevelList;
        }

        public string ManipulateUserLevel(SEC_UserLevel userLevel, string action, out string errorNumber)
        {
            errorNumber = string.Empty;
            try
            {
                var factory = new DatabaseProviderFactory();
                var db = factory.CreateDefault() as SqlDatabase;
                using (var dbCommandWrapper = db.GetStoredProcCommand("SetUserLevel"))
                {
                    // Set parameters
                    db.AddInParameter(dbCommandWrapper, "@UserLevelID", SqlDbType.VarChar, userLevel.ID);
                    db.AddInParameter(dbCommandWrapper, "@UserLevel", SqlDbType.Int, userLevel.UserLevel);
                    db.AddInParameter(dbCommandWrapper, "@UserLevelName", SqlDbType.VarChar, userLevel.UserLevelName);
                    db.AddInParameter(dbCommandWrapper, "@UserLevelSL", SqlDbType.VarChar, userLevel.UserLevelSL);
                    db.AddInParameter(dbCommandWrapper, "@UserLevelType", SqlDbType.VarChar, userLevel.UserLevelType);
                    db.AddInParameter(dbCommandWrapper, "@SetBy", SqlDbType.VarChar, userLevel.SetBy);
                    db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.VarChar, userLevel.ModifiedBy);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, userLevel.Status);
                    db.AddInParameter(dbCommandWrapper, "@Action", SqlDbType.VarChar, action);
                    db.AddInParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
                    // Execute SP.
                    db.ExecuteNonQuery(dbCommandWrapper);
                    // Getting output parameters and setting response details.
                    if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                    {
                        // Get the error number, if error occurred.
                        errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                    }
                }
            }
            catch
            {
                errorNumber = "E404"; // Log ex.Message  Insert Log Table             
            }
            return errorNumber;
        }

        #endregion
        
    }
}

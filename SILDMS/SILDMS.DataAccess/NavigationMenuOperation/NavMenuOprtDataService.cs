using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.NavigationMenuOperation;
using SILDMS.Model.DocScanningModule;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccess.NavigationMenuOperation
{
    public class NavMenuOprtDataService : INavMenuOprtDataService
    {
        #region Fields

        private readonly string spErrorParam = "@p_Error";
        private readonly string spStatusParam = "@p_Status";

        #endregion
        public List<SEC_MenuOperation> GetMenuOperation(string id, string action, out string errorNumber)
        {
            errorNumber = string.Empty;
            var menuOperationList = new List<SEC_MenuOperation>();
            var factory = new DatabaseProviderFactory();
            var db = factory.CreateDefault() as SqlDatabase;
            using (var dbCommandWrapper = db.GetStoredProcCommand("GetMenuOperation"))
            {
                db.AddInParameter(dbCommandWrapper, "@MenuOperationID", SqlDbType.VarChar, id);

                db.AddOutParameter(dbCommandWrapper, spErrorParam, DbType.Int32, 10);

                var ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, "@p_Error").IsNullOrZero())
                {
                    // Get the error number, if error occurred.
                    errorNumber = db.GetParameterValue(dbCommandWrapper, "@p_Error").PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var dt1 = ds.Tables[0];
                        menuOperationList = dt1.AsEnumerable().Select(reader => new SEC_MenuOperation
                        {
                            MenuOperationID = reader.GetString("MenuOperationID"),
                            MenuOperationTitle = reader.GetString("MenuOperationTitle"),
                            DefaultValue = reader.GetBoolean("DefaultValue"),
                            MenuOperationSL = reader.GetString("MenuOperationSL"),
                            OwnerID = reader.GetString("OwnerID"),
                            SetOn = string.Format("{0:dd/mm/yyyy}", Convert.ToString(reader.GetDateTime("SetOn"), CultureInfo.InvariantCulture)),
                            SetBy = reader.GetString("SetBy"),
                            ModifiedOn = string.Format("{0:dd/mm/yyyy}", reader.GetDateTime("ModifiedOn").ToString(CultureInfo.InvariantCulture)),
                            ModifiedBy = reader.GetString("ModifiedBy"),
                            Status = reader.GetInt16("Status"),
                            UserLevel = reader.GetInt32("UserLevel")
                        }).ToList();
                    }
                }
            }
            return menuOperationList;
        }

        public string ManipulateMenuOperation(SEC_MenuOperation menuOpteration, string action, out string errorNumber)
        {
            errorNumber = string.Empty;
            try
            {
                var factory = new DatabaseProviderFactory();
                var db = factory.CreateDefault() as SqlDatabase;
                using (var dbCommandWrapper = db.GetStoredProcCommand("SetMenuOperation"))
                {
                    // Set parameters 
                    db.AddInParameter(dbCommandWrapper, "@MenuOperationID", SqlDbType.NVarChar, menuOpteration.MenuOperationID.Trim());
                    db.AddInParameter(dbCommandWrapper, "@MenuOperationTitle", SqlDbType.NVarChar, menuOpteration.MenuOperationTitle.Trim());
                    db.AddInParameter(dbCommandWrapper, "@DefaultValue", SqlDbType.Bit, menuOpteration.DefaultValue);
                    db.AddInParameter(dbCommandWrapper, "@MenuOperationSL", SqlDbType.NVarChar, menuOpteration.MenuOperationSL.Trim());
                    db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, menuOpteration.OwnerID);
                    db.AddInParameter(dbCommandWrapper, "@UserLevel", SqlDbType.Int, menuOpteration.UserLevel);
                    db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, menuOpteration.SetBy);
                    db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, menuOpteration.ModifiedBy);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, menuOpteration.Status);
                    db.AddInParameter(dbCommandWrapper, "@Action", SqlDbType.VarChar, action);
                    db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
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
            catch (Exception ex)
            {
                errorNumber = "E404"; // Log ex.Message  Insert Log Table               
            }
            return errorNumber;
        }
    }
}

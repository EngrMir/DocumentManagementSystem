using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.NavMenuOptSetup;
using SILDMS.Model.SecurityModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.DataAccess.NavMenuOptSetup
{
    public class NavMenuOptSetupDataService : INavMenuOptSetupDataService
    {
        private readonly string spStatusParam = "@p_Status";
        public List<SEC_NavMenuOptSetup> GetNavMenuOptSetup(string ownerID, out string errorNumber)
        {
            errorNumber = string.Empty;
            List<SEC_NavMenuOptSetup> navMenuLst = new List<SEC_NavMenuOptSetup>();
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetAllMenuDetails"))
            {
                // Set parameters 
                db.AddInParameter(dbCommandWrapper, "@MenuId", SqlDbType.VarChar, "");
                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.VarChar, ownerID);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, DbType.String, 10);
                // Execute SP.
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    // Get the error number, if error occurred.
                    errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                }
                else
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = ds.Tables[0];
                        navMenuLst = dt1.AsEnumerable().Select(reader => new SEC_NavMenuOptSetup
                        {
                            MenuID = reader.GetString("MenuId"),
                            MenuTitle = reader.GetString("MenuTitle"),
                            MenuUrl = reader.GetString("MenuUrl"),
                            ParentMenuID = reader.GetString("ParentMenuID"),
                            MenuIcon = reader.GetString("MenuIcon"),
                            MenuOrder = reader.GetInt32("MenuOrder"),
                            SetBy = reader.GetString("SetBy"),
                            SetOn = reader.GetString("SetOn"),
                            check = reader.GetString("Checked"),
                            Status = reader.GetInt32("Status")
                        }).ToList();
                    }

                   
                }
                return navMenuLst;
            }
        }


        public string SetMenuOperations(SEC_MenuDetails menuDetails, string action, out string errorNumber)
        {
            errorNumber = String.Empty;
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;
                using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetMenuOperation"))
                {
                    // Set parameters 
                    db.AddInParameter(dbCommandWrapper, "@MenuID", SqlDbType.NVarChar, menuDetails.MenuID.Trim());
                    db.AddInParameter(dbCommandWrapper, "@VisibleInRollMenu", SqlDbType.Bit, true);
                    db.AddInParameter(dbCommandWrapper, "@OwnerID ", SqlDbType.NVarChar, menuDetails.Owner.OwnerID.Trim());
                    db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, menuDetails.SetBy);
                    db.AddInParameter(dbCommandWrapper, "@UserLevel", SqlDbType.Int, menuDetails.UserLevel);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, menuDetails.Status);
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

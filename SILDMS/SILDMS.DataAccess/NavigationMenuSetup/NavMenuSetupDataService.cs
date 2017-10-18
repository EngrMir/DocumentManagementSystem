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
using SILDMS.DataAccessInterface.NavigationMenuSetup;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccess.NavigationMenuSetup
{
    public class NavMenuSetupDataService : INavMenuSetupDataService
    {

        #region Variables

        private readonly string spErrorParam = "@p_Error";
        private readonly string spStatusParam = "@p_Status";

        #endregion

        public List<SEC_Menu> GetMenus(string id, string action, out string errorNumber)
        {
            errorNumber = string.Empty;
            var menuList = new List<SEC_Menu>();
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetMenu"))
            {
                db.AddInParameter(dbCommandWrapper, "@DocCategoryID", SqlDbType.VarChar, id);

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
                        DataTable dt1 = ds.Tables[0];
                        menuList = dt1.AsEnumerable().Select(reader => new SEC_Menu
                        {
                            MenuID = reader.GetString("MenuID"),
                            MenuTitle = reader.GetString("MenuTitle"),
                            ParentMenuID = reader.GetString("ParentMenuID"),
                            MenuUrl = reader.GetString("MenuUrl"),
                            MenuIcon = reader.GetString("MenuIcon"),
                            MenuOrder = reader.GetInt32("MenuOrder"),
                            TotalUserAllowed = reader.GetInt32("TotalUserAllowed"),
                            ConcurrentUser = reader.GetInt32("ConcurrentUser"),
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
            return menuList;
        }

        public string ManipulateMenu(SEC_Menu menu, string action, out string errorNumber)
        {
            errorNumber = String.Empty;
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;
                using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocCategory"))
                {
                    // Set parameters 
                    db.AddInParameter(dbCommandWrapper, "@MenuID", SqlDbType.NVarChar, menu.MenuID);
                    db.AddInParameter(dbCommandWrapper, "@MenuTitle", SqlDbType.NVarChar, menu.MenuTitle);
                    db.AddInParameter(dbCommandWrapper, "@ParentMenuID", SqlDbType.NVarChar, menu.ParentMenuID);
                    db.AddInParameter(dbCommandWrapper, "@MenuUrl", SqlDbType.NVarChar, menu.MenuUrl);
                    db.AddInParameter(dbCommandWrapper, "@MenuIcon", SqlDbType.NVarChar, menu.MenuIcon);
                    db.AddInParameter(dbCommandWrapper, "@MenuOrder", SqlDbType.Int, menu.MenuOrder);
                    db.AddInParameter(dbCommandWrapper, "@TotalUserAllowed", SqlDbType.Int, menu.TotalUserAllowed);
                    db.AddInParameter(dbCommandWrapper, "@ConcurrentUser", SqlDbType.Int, menu.ConcurrentUser);
                    db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, menu.OwnerID);
                    db.AddInParameter(dbCommandWrapper, "@UserLevel", SqlDbType.Int, menu.UserLevel);
                    db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, menu.SetBy);
                    db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, menu.ModifiedBy);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, menu.Status);
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

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.Menu;
using SILDMS.Model.SecurityModule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SILDMS.DataAccess.Menu
{
    public class MenuDataService : IMenuDataService
    {
        private readonly string spErrorParam = "@p_Status";
        public List<SEC_Menu> GetMenu(string ownerID,string menuID, string action, out string errorNumber)
        {

            errorNumber = string.Empty;
            List<SEC_Menu> userList = new List<SEC_Menu>();
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetAllMenu"))
            {
                // Set parameters 
                db.AddInParameter(dbCommandWrapper, "@MenuId", SqlDbType.VarChar, menuID);
                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.VarChar, ownerID);
               // db.AddInParameter(dbCommandWrapper, "@Action", SqlDbType.VarChar, action);
                db.AddOutParameter(dbCommandWrapper, spErrorParam, DbType.Int32, 10);
                // Execute SP.
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, "@p_Status").IsNullOrZero())
                {
                    // Get the error number, if error occurred.
                    errorNumber = db.GetParameterValue(dbCommandWrapper, "@p_Status").PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = ds.Tables[0];
                        userList = dt1.AsEnumerable().Select(reader => new SEC_Menu
                        {
                            MenuID = reader.GetString("MenuID"),
                            MenuTitle = reader.GetString("MenuTitle"),
                            MenuUrl = reader.GetString("MenuUrl"),
                            ParentMenuID = reader.GetString("ParentMenuID"),
                            MenuIcon = reader.GetString("MenuIcon"),
                            MenuOrder = reader.GetInt32("MenuOrder"),

                            TotalUserAllowed = reader.GetInt32("TotalUserAllowed"),
                            ConcurrentUser = reader.GetInt32("ConcurrentUser"),
                            OwnerID = reader.GetString("OwnerID"),
                            UserLevel = reader.GetInt32("UserLevel"),
                            SetOn = reader.GetString("SetOn"),
                            SetBy = reader.GetString("SetBy"),
                            ModifiedOn = reader.GetString("ModifiedOn"),
                            ModifiedBy = reader.GetString("ModifiedBy"),
                            Status = reader.GetInt32("Status")
                        }).ToList();
                    }
                }
            }
            return userList;
        }

        public string AddMenu(SEC_Menu menu, string action, out string errorNumber)
        {
            errorNumber = string.Empty;
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;
                using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetNavigationMenu"))
                {
                    // Set parameters 
                    db.AddInParameter(dbCommandWrapper, "@MenuID", SqlDbType.NVarChar, menu.MenuID);
                    db.AddInParameter(dbCommandWrapper, "@MenuTitle", SqlDbType.NVarChar, menu.MenuTitle.Trim());
                    db.AddInParameter(dbCommandWrapper, "@MenuUrl", SqlDbType.NVarChar, menu.MenuUrl.Trim());
                    db.AddInParameter(dbCommandWrapper, "@ParentMenuID ", SqlDbType.NVarChar, menu.ParentMenu.MenuID.Trim());
                    db.AddInParameter(dbCommandWrapper, "@MenuIcon", SqlDbType.NVarChar, menu.MenuIcon.Trim());
                    db.AddInParameter(dbCommandWrapper, "@MenuOrder", SqlDbType.Int, menu.MenuOrder == null ? 0 : menu.MenuOrder);

                    db.AddInParameter(dbCommandWrapper, "@TotalUserAllowed", SqlDbType.Int, menu.TotalUserAllowed == null ? 0 : menu.TotalUserAllowed);
                    db.AddInParameter(dbCommandWrapper, "@ConcurrentUser", SqlDbType.Int, menu.ConcurrentUser == null ? 0 : menu.ConcurrentUser);
                    db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, menu.Owner.OwnerID);
                    db.AddInParameter(dbCommandWrapper, "@UserLevel", SqlDbType.Int, menu.UserLevel);

                    db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, menu.SetBy);
                    db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, menu.ModifiedBy);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, menu.Status);
                    db.AddInParameter(dbCommandWrapper, "@Action", SqlDbType.NVarChar, action);

                    db.AddOutParameter(dbCommandWrapper, spErrorParam, SqlDbType.VarChar, 10);         
                    // Execute SP.
                    db.ExecuteNonQuery(dbCommandWrapper);
                    // Getting output parameters and setting response details.
                    if (!db.GetParameterValue(dbCommandWrapper, spErrorParam).IsNullOrZero())
                    {
                        // Get the error number, if error occurred.
                        errorNumber = db.GetParameterValue(dbCommandWrapper, spErrorParam).PrefixErrorCode();
                    }
                    else
                    {
                        errorNumber = "E2000";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log ex.Message
                errorNumber = "E4004";
            }
            return errorNumber;
        }

        public string EditMenu(SEC_Menu menu, string action, out string errorNumber)
        {
            throw new NotImplementedException();
        }

        public string DeleteMenu(SEC_Menu menu, string action, out string errorNumber)
        {
            errorNumber = string.Empty;
           
            return errorNumber;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.RoleMenuPermission;
using SILDMS.Model.DocScanningModule;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccess.RoleMenuPermission
{
    public class RoleMenuPermissionDataService: IRoleMenuPermissionDataService
    {
        private readonly string spStatusParam = "@p_Status";


        public List<SEC_RoleMenuPermission> GetRoleMenuPermission(string _UserID, string _RoleID,
            out string errorNumber)
        {
            errorNumber = string.Empty;
            List<SEC_RoleMenuPermission> docPropIdentifyList = new List<SEC_RoleMenuPermission>();
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetRoleMenuPermission"))
            {
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, _UserID);
                db.AddInParameter(dbCommandWrapper, "@RoleID", SqlDbType.VarChar, _RoleID);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, DbType.String, 10);
                
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = ds.Tables[0];
                        docPropIdentifyList = dt1.AsEnumerable().Select(reader => new SEC_RoleMenuPermission
                        {
                            RoleMenuPermissionID = reader.GetString("RoleMenuPermissionID"),
                            RoleID = reader.GetString("RoleID"),
                            MenuID = reader.GetString("MenuID"),
                            MenuTitle = reader.GetString("MenuTitle"),
                            RoleParentMenuID = reader.GetString("RoleParentMenuID"),
                            OwnerID = reader.GetString("OwnerID"),
                            UserLevel = reader.GetString("UserLevel"),
                            SetBy = reader.GetString("SetBy"),
                            ModifiedBy = reader.GetString("ModifiedBy"),
                            Status = reader.GetInt16("Status")
                        }).ToList();
                    }
                }
            }
            return docPropIdentifyList;
        }
        public List<SEC_NavMenuOptSetup> GetOwnerPermittedMenu(string _UserID, string ownerID,
            out string errorNumber)
        {
            errorNumber = string.Empty;
            List<SEC_NavMenuOptSetup> navMenuLst = new List<SEC_NavMenuOptSetup>();
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetOwnerPermittedMenu"))
            {
                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.VarChar, ownerID);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, DbType.String, 10);
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
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
                            Status = reader.GetInt32("Status")
                        }).ToList();
                    }
                }
                return navMenuLst;
            }
        }

        public string SetRoleMenuPermission(SEC_RoleMenuPermission model, out string errorNumber)
        {
            errorNumber = String.Empty;
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;
                using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetRoleMenuPermission"))
                {
                    db.AddInParameter(dbCommandWrapper, "@RoleID", SqlDbType.NVarChar, model.Role.RoleID);
                    db.AddInParameter(dbCommandWrapper, "@MenuID", SqlDbType.NVarChar, model.MenuID);
                    db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.Owner.OwnerID);
                    db.AddInParameter(dbCommandWrapper, "@UserLevel ", SqlDbType.NVarChar, model.UserLevel);
                    db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.SetBy);
                    db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
                    db.ExecuteNonQuery(dbCommandWrapper);
                    
                    if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                    {
                        errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                    }
                }
            }
            catch (Exception ex)
            {
                errorNumber = "E404";              
            }
            return errorNumber;
        }
    }
}

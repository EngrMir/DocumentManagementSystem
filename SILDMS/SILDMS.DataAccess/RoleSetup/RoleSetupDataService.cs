using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.RoleSetup;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccess.RoleSetup
{
    public class RoleSetupDataService: IRoleSetupDataService
    {
        private readonly string spStatusParam = "@p_Status";
        public List<SEC_Role> GetRole(string _UserID, string _roleID, out string _errorNumber)
        {
            _errorNumber = string.Empty;
            List<SEC_Role> roleList = new List<SEC_Role>();

            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetRole"))
            {
                
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, _UserID);
                db.AddInParameter(dbCommandWrapper, "@RoleID", SqlDbType.VarChar, _roleID);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, DbType.String, 10);
                
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    _errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = ds.Tables[0];
                        roleList = dt1.AsEnumerable().Select(reader => new SEC_Role
                        {
                            RoleID = reader.GetString("RoleID"),
                            OwnerID = reader.GetString("OwnerID"),
                            RoleDescription = reader.GetString("RoleDescription"),
                            RoleTitle = reader.GetString("RoleTitle"),
                            RoleType = reader.GetString("RoleType"),
                            UserLevel = reader.GetString("UserLevel"),
                            SetOn = reader.GetString("SetOn"),
                            SetBy = reader.GetString("SetBy"),
                            ModifiedOn = reader.GetString("ModifiedOn"),
                            ModifiedBy = reader.GetString("ModifiedBy"),
                            Status = reader.GetInt32("Status"),
                        }).ToList();
                    }
                }
            }
            return roleList;
        }

        public string AddRole(SEC_Role modelRole, string action, out string errorNumber)
        {
            errorNumber = String.Empty;
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;
                using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetRole"))
                {
                    db.AddInParameter(dbCommandWrapper, "@RoleID", SqlDbType.NVarChar, modelRole.RoleID);

                    db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, modelRole.Owner.OwnerID);


                    db.AddInParameter(dbCommandWrapper, "@RoleTitle", SqlDbType.NVarChar, modelRole.RoleTitle);
                    db.AddInParameter(dbCommandWrapper, "@RoleDescription", SqlDbType.NVarChar, modelRole.RoleDescription);
                    db.AddInParameter(dbCommandWrapper, "@RoleType ", SqlDbType.NVarChar, modelRole.RoleType);
                    db.AddInParameter(dbCommandWrapper, "@UserLevel", SqlDbType.NVarChar, modelRole.UserLevel);

                    db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, modelRole.SetBy);
                    db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, modelRole.ModifiedBy);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, modelRole.Status);
                    db.AddInParameter(dbCommandWrapper, "@Action", SqlDbType.VarChar, action);
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

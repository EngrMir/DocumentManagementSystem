using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.OwnerLevelPermission;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccess.OwnerLevelPermission
{
    public class OwnerLevelPermissionDataService: IOwnerLevelPermissionDataService
    {
        private readonly string spStatusParam = "@p_Status";
        public List<SEC_UserOwnerAccess> GetAllUserOwnerAccess(string _UserID,
            out string _errorNumber)
        {
            _errorNumber = string.Empty;
            List<SEC_UserOwnerAccess> userOwnerAccesses = new List<SEC_UserOwnerAccess>();

            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetAllUserOwnerAccess"))
            {
                // Set parameters 
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, _UserID);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, DbType.String, 10);
                // Execute SP.
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    // Get the error number, if error occurred.
                    _errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = ds.Tables[0];
                        userOwnerAccesses = dt1.AsEnumerable().Select(reader => new SEC_UserOwnerAccess
                        {
                            UserOwnerAccessID = reader.GetString("UserOwnerAccessID"),
                            UserID = reader.GetString("UserID"),
                            PermittedOwnerID = reader.GetString("PermittedOwnerID"),
                            OwnerAccessPower = reader.GetString("OwnerAccessPower"),
                            AccessTimeLimit = reader.GetString("AccessTimeLimit"),
                            Remarks = reader.GetString("Remarks"),
                            SetOn = reader.GetString("SetOn"),
                            SetBy = reader.GetString("SetBy"),
                            ModifiedOn = reader.GetString("ModifiedOn"),
                            ModifiedBy = reader.GetString("ModifiedBy"),
                            Status = reader.GetInt32("Status"),
                            UserLevel = reader.GetString("UserLevelID"),
                            SupervisorLevel = reader.GetString("SupervisorLevel")
                        }).ToList();
                    }
                }
            }
            return userOwnerAccesses;
        }


        public List<UserWisePermittedOwner> GetUserWisePermittedOwnerList(string _UserID, string _SessionUserID, 
            string _OwnerLevelID, out string _errorNumber)
        {
            _errorNumber = string.Empty;
            List<UserWisePermittedOwner> userWisePermittedOwners = new List<UserWisePermittedOwner>();

            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetUserWisePermittedOwnerList"))
            {
                // Set parameters 
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, _UserID);
                db.AddInParameter(dbCommandWrapper, "@SessionUserID", SqlDbType.VarChar, _SessionUserID);
                db.AddInParameter(dbCommandWrapper, "@OwnerLevelID", SqlDbType.VarChar, _OwnerLevelID);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, DbType.String, 10);
                // Execute SP.
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    // Get the error number, if error occurred.
                    _errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = ds.Tables[0];
                        userWisePermittedOwners = dt1.AsEnumerable().Select(reader => new UserWisePermittedOwner
                        {
                            OwnerID = reader.GetString("OwnerID"),
                            OwnerName = reader.GetString("OwnerName"),
                            IsSelected = reader.GetBoolean("Status"),
                            UserLevel = reader.GetString("UserLevelID"),
                            SupervisorLevel = reader.GetString("SupervisorLevel"),
                            UserOwnerAccessID = reader.GetString("UserOwnerAccessID")
                        }).ToList();
                    }
                }
            }
            return userWisePermittedOwners;
        }

        public string SetOwnerLevelPermission(SEC_UserOwnerAccess model, out string errorNumber)
        {
            errorNumber = String.Empty;
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;
                //if (model.OwnerModel != null)
                //{
                    DataTable OwnerDataTable = new DataTable();
                    OwnerDataTable.Columns.Add("OwnerID");
                    OwnerDataTable.Columns.Add("UserLevel");
                    OwnerDataTable.Columns.Add("SupervisorLevel");
                    OwnerDataTable.Columns.Add("UserOwnerAccessID");

                if (model.OwnerModel != null)
                {
                    foreach (var item in model.OwnerModel)
                    {
                        DataRow objDataRow = OwnerDataTable.NewRow();

                        objDataRow[0] = item.OwnerID;
                        objDataRow[1] = item.UserLevel;
                        objDataRow[2] = item.SupervisorLevel;
                        objDataRow[3] = item.UserOwnerAccessID;

                        OwnerDataTable.Rows.Add(objDataRow);
                    }
                }

                using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetOwnerLevelPermission"))
                    {
                        // Set parameters 
                        db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.UserID);
                        db.AddInParameter(dbCommandWrapper, "@EnableOwnerSecurity", SqlDbType.NVarChar, model.EnableOwnerSecurity);
                        db.AddInParameter(dbCommandWrapper, "@OwnerLevelAccessID", SqlDbType.NVarChar, model.OwnerLevelAccessID);
                        db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.Structured, OwnerDataTable);

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
                //}

            }
            catch (Exception ex)
            {
                errorNumber = "E404"; // Log ex.Message  Insert Log Table               
            }
            return errorNumber;
        }

        

       
    }
}

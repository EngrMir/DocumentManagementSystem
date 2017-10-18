using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.Users;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = SILDMS.Model.Apps_User;

namespace SILDMS.DataAccess.Users
{
    public class UserDataService : IUserDataService
    {
        private readonly string spStatusParam = "@p_Status";

        public List<SEC_User> GetAllUser(string id, string ownerID, out string errorNumber)
        {
            errorNumber = string.Empty;
            List<SEC_User> userList = new List<SEC_User>();
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetUserDetails"))
            {
                // Set parameters 
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, id);
                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, ownerID);
                db.AddOutParameter(dbCommandWrapper, "@p_Error", DbType.Int32, 10);
                // Execute SP.
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

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
                        userList = dt1.AsEnumerable().Select(reader => new SEC_User
                        {
                            UserID = reader.GetString("UserID"),
                            OwnerLevelID = reader.GetString("OwnerLevelID"),
                            LevelName = reader.GetString("LevelName"),
                            OwnerID = reader.GetString("OwnerID"),
                            OwnerName = reader.GetString("OwnerName"),
                            RoleTitle = reader.GetString("RoleTitle"),
                            RoleID = reader.GetString("RoleID"),
                            EmployeeID = reader.GetString("EmployeeID"),
                            UserFullName = reader.GetString("UserFullName"),
                            UserDesignation = reader.GetString("UserDesignation"),
                            JobLocation = reader.GetString("JobLocation"),
                            UserNo = reader.GetString("UserNo"),
                            UserName = reader.GetString("UserName"),
                            PermissionLevel = reader.GetString("PermissionLevel"),
                            AccessOwnerLevel = reader.GetString("AccessOwnerLevel"),
                            UserLevelID = reader.GetString("UserLevelID"),
                            AccessDataLevel = reader.GetString("AccessDataLevel"),
                            DocClassification = reader.GetString("DocClassification"),
                            ClassificationLevel = reader.GetString("ClassificationLevel"),
                            SecurityStatus = reader.GetString("SecurityStatus"),
                            DateLimit = string.Format(reader.GetDateTime("DateLimit").ToShortDateString(),"dd/MM/yyyy"),
                            DefaultServer = reader.GetString("DefaultServer"),
                            IntMailAddress = reader.GetString("IntMailAddress"),
                            IntmailStatus = reader.GetString("IntmailStatus"),
                            ExtMailAddress = reader.GetString("ExtMailAddress"),
                            ExtMailStatus = reader.GetString("ExtMailStatus"),
                            UserPicture = reader.GetString("UserPicture"),
                            SupervisorLevel = reader.GetString("SupervisorLevel"),
                            Remarks = reader.GetString("Remarks"),
                            SetOn = string.Format("{0:dd/mm/yyyy}", Convert.ToString(reader.GetDateTime("SetOn"), CultureInfo.InvariantCulture)),
                            AccessDataLevelName = reader.GetString("AccessDataLevelName"),
                            UserLevelName = reader.GetString("UserLevelName"),
                            SupervisorLevelName = reader.GetString("SupervisorLevelName"),
                            ClassificationLevelName = reader.GetString("ClassificationLevelName"),
                            DocClassificationName = reader.GetString("DocClassificationName"),
                            ContactNo = reader.GetString("ContactNo"),
                            MessageStatus = reader.GetString("MessageStatus"),
                            UserPassword = reader.GetString("UserPassword"),
                            Status = reader.GetString("Status")
                        }).ToList();
                    }
                }
            }
            return userList;
        }
        public string AddUser(SEC_User objUser, string action, out string errorNumber)
        {
            errorNumber = String.Empty;
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;
                using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetUsers"))
                {
                    // Set parameters 
                    db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, objUser.UserID);
                    db.AddInParameter(dbCommandWrapper, "@OwnerLevelID", SqlDbType.NVarChar, objUser.OwnerLevelID==null?"":objUser.OwnerLevelID.Trim());
                    db.AddInParameter(dbCommandWrapper, "@OwnerID ", SqlDbType.NVarChar, objUser.OwnerID == null ? "" : objUser.OwnerID.Trim());
                    db.AddInParameter(dbCommandWrapper, "@EmployeeID", SqlDbType.NVarChar, objUser.EmployeeID == null ? "" : objUser.EmployeeID);
                    db.AddInParameter(dbCommandWrapper, "@UserFullName", SqlDbType.NVarChar, objUser.UserFullName);
                    db.AddInParameter(dbCommandWrapper, "@UserDesignation ", SqlDbType.NVarChar, objUser.UserDesignation);
                    db.AddInParameter(dbCommandWrapper, "@JobLocation", SqlDbType.NVarChar, objUser.JobLocation);
                    db.AddInParameter(dbCommandWrapper, "@UserNo", SqlDbType.NVarChar, objUser.UserNo);

                    db.AddInParameter(dbCommandWrapper, "@UserName", SqlDbType.NVarChar, objUser.UserName == null?"": objUser.UserName.Trim());
                    db.AddInParameter(dbCommandWrapper, "@UserPassword", SqlDbType.NVarChar, objUser.UserPassword.Trim());
                    db.AddInParameter(dbCommandWrapper, "@RoleID ", SqlDbType.NVarChar, objUser.RoleID == null ? "" : objUser.RoleID.Trim());
                    db.AddInParameter(dbCommandWrapper, "@PermissionLevel", SqlDbType.NVarChar, objUser.PermissionLevel);
                    db.AddInParameter(dbCommandWrapper, "@UserLevelID", SqlDbType.Int, objUser.UserLevelID == null ? 0 : Convert.ToInt32(objUser.UserLevelID));
                    db.AddInParameter(dbCommandWrapper, "@SupervisorLevel ", SqlDbType.Int, objUser.SupervisorLevel == null ? 0 : Convert.ToInt32(objUser.SupervisorLevel));
                    db.AddInParameter(dbCommandWrapper, "@AccessOwnerLevel", SqlDbType.NVarChar, DataValidation.TrimmedOrDefault(objUser.AccessOwnerLevel) );
                    db.AddInParameter(dbCommandWrapper, "@AccessDataLevel", SqlDbType.Int, objUser.AccessDataLevel == null ? 0 : Convert.ToInt32(objUser.AccessDataLevel));

                    db.AddInParameter(dbCommandWrapper, "@DocClassification", SqlDbType.Int, objUser.DocClassification == null ? 0 : Convert.ToInt32(objUser.DocClassification));
                    db.AddInParameter(dbCommandWrapper, "@ClassificationLevel", SqlDbType.Int, objUser.ClassificationLevel == null ? 0 : Convert.ToInt32(objUser.ClassificationLevel));
                    db.AddInParameter(dbCommandWrapper, "@SecurityStatus ", SqlDbType.NVarChar, objUser.SecurityStatus == null ? "" : objUser.SecurityStatus.Trim());
                    db.AddInParameter(dbCommandWrapper, "@DateLimit", SqlDbType.DateTime, objUser.DateLimit);
                    db.AddInParameter(dbCommandWrapper, "@DefaultServer", SqlDbType.NVarChar, objUser.DefaultServer);
                    db.AddInParameter(dbCommandWrapper, "@IntMailAddress ", SqlDbType.NVarChar, objUser.IntMailAddress);
                    db.AddInParameter(dbCommandWrapper, "@IntmailStatus", SqlDbType.NVarChar, objUser.IntmailStatus);
                    db.AddInParameter(dbCommandWrapper, "@ExtMailAddress", SqlDbType.NVarChar, objUser.ExtMailAddress);
                    db.AddInParameter(dbCommandWrapper, "@ExtMailStatus", SqlDbType.NVarChar, objUser.ExtMailStatus);

                    db.AddInParameter(dbCommandWrapper, "@ContactNo", SqlDbType.NVarChar, objUser.ContactNo);
                    db.AddInParameter(dbCommandWrapper, "@MessageStatus", SqlDbType.NVarChar, objUser.MessageStatus==null?"": objUser.MessageStatus.Trim());

                    db.AddInParameter(dbCommandWrapper, "@UserPicture ", SqlDbType.NVarChar, objUser.UserPicture == null ? "/noimage.jpg" : objUser.UserPicture.Trim());
                    db.AddInParameter(dbCommandWrapper, "@Remarks", SqlDbType.NVarChar, objUser.Remarks);

                    db.AddInParameter(dbCommandWrapper, "@SetOn ", SqlDbType.DateTime, DateTime.Now);
                    db.AddInParameter(dbCommandWrapper, "@SetBy", SqlDbType.NVarChar, objUser.SetBy);
                    db.AddInParameter(dbCommandWrapper, "@ModifiedOn", SqlDbType.DateTime, DateTime.Now);
                    db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, objUser.ModifiedBy);
                    db.AddInParameter(dbCommandWrapper, "@Action", SqlDbType.VarChar, action);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, objUser.Status);
                    db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.NVarChar, 10);
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
        public bool IsValidUser(string user, string password, string ip, out List<GetUserAccessPermission_Result> accessList)
        {
            bool errorNumber = false;
            accessList = new List<GetUserAccessPermission_Result>();
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("CheckUserValidity"))
            {
                // Set parameters 
                db.AddInParameter(dbCommandWrapper, "@user", SqlDbType.NVarChar, user.Trim());
                db.AddInParameter(dbCommandWrapper, "@password", SqlDbType.NVarChar, password.Trim());
                db.AddInParameter(dbCommandWrapper, "@userIP", SqlDbType.NVarChar, ip);
                db.AddOutParameter(dbCommandWrapper, "@p_Status", DbType.Int32, 0);
                // Execute SP.
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, "@p_Status").IsNullOrZero())
                {
                    // Get the error number, if error occurred.
                    errorNumber = db.GetParameterValue(dbCommandWrapper, "@p_Status").PrefixErrorCode().StringToBool();
                    if (errorNumber && ds.Tables[0].Rows.Count == 0)
                    {
                        errorNumber = false;
                    }
                }
                if (ds.Tables[0].Rows.Count > 0 && (ds.Tables[1].Rows.Count > 0))
                {
                    DataTable dt1 = ds.Tables[0];
                    DataTable dt2 = ds.Tables[1];
                    accessList = dt1.AsEnumerable().Select(reader => new GetUserAccessPermission_Result
                    {
                        UserID = reader.GetString("UserID"),
                        UserName = reader.GetString("UserName"),
                        OwnerLevelID = reader.GetString("OwnerLevelID"),
                        LevelName = reader.GetString("LevelName"),
                        OwnerID = reader.GetString("OwnerID"),
                        OwnerName = reader.GetString("OwnerName"),
                        RoleID = reader.GetString("RoleID"),
                        RoleTitle = reader.GetString("RoleTitle"),
                        DefaultServer = reader.GetString("DefaultServer"),
                        AccessMenu = dt2.AsEnumerable().Select(reader2 => new SEC_Menu
                        {
                            MenuID = reader2.GetString("MenuID"),
                            MenuTitle = reader2.GetString("MenuTitle"),
                            MenuUrl = reader2.GetString("MenuUrl"),
                            PermissionClass = reader2.GetString("PermissionClass"),
                            MenuIcon = reader2.GetString("MenuIcon"),
                            ParentMenuID = reader2.GetString("ParentMenuID"),

                        }).ToList()
                    }).ToList();
                }
            }

            return errorNumber;
        }
    }
}

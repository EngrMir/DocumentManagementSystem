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
using SILDMS.DataAccessInterface.Owner;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccess.Owner
{
    public class OwnerDataService : IOwnerDataService
    {
        private readonly string spErrorParam = "@p_Error";
        private readonly string spStatusParam = "@p_Status";
        public List<DSM_Owner> GetOwner(string id, string action, out string errorNumber)
        {
            errorNumber = string.Empty;
            List<DSM_Owner> ownersList = new List<DSM_Owner>();
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetOwner"))
            {
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, action);
                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.VarChar, id);
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
                        ownersList = dt1.AsEnumerable().Select(reader => new DSM_Owner
                        {
                            OwnerID = reader.GetString("OwnerID"),
                            UDOwnerCode = reader.GetString("UDOwnerCode"),
                            OwnerLevelID = reader.GetString("OwnerLevelID"),
                            LevelName = reader.GetString("LevelName"),
                            OwnerName = reader.GetString("OwnerName"),
                            OwnerShortName = reader.GetString("OwnerShortName"),
                            ParentOwner = reader.GetString("ParentOwner"),
                            ParentName = reader.GetString("ParentName"),
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
            return ownersList;
        }

        public string ManipulateOwner(DSM_Owner owner, string action, out string errorNumber)
        {
            errorNumber = String.Empty;
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;
                using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetOwner"))
                {
                    // Set parameters 
                    db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, owner.OwnerID);
                    db.AddInParameter(dbCommandWrapper, "@UDOwnerCode", SqlDbType.NVarChar, owner.UDOwnerCode.Trim());
                    db.AddInParameter(dbCommandWrapper, "@OwnerLevelID", SqlDbType.NVarChar, owner.OwnerLevelID);
                    db.AddInParameter(dbCommandWrapper, "@OwnerName", SqlDbType.NVarChar, owner.OwnerName.Trim());
                    db.AddInParameter(dbCommandWrapper, "@OwnerShortName", SqlDbType.NVarChar, owner.OwnerShortName.Trim());
                    db.AddInParameter(dbCommandWrapper, "@ParentOwner", SqlDbType.NVarChar, owner.ParentOwner);
                    db.AddInParameter(dbCommandWrapper, "@UserLevel", SqlDbType.Int, owner.UserLevel);
                    db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, owner.SetBy);
                    db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, owner.ModifiedBy);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, owner.Status);
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



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
using SILDMS.DataAccessInterface.DocumentCategory;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;

namespace SILDMS.DataAccess.DocumentCategory
{
    public class DocCategoryDataService : IDocCategoryDataService
    {
        private readonly string spErrorParam = "@p_Error";
        private readonly string spStatusParam = "@p_Status";
        public List<DSM_DocCategory> GetDocCategory(string id, string userId, out string errorNumber)
        {
            errorNumber = string.Empty;
            List<DSM_DocCategory> docCategoriesList = new List<DSM_DocCategory>();
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetDocCategory"))
            {
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, userId);
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
                        docCategoriesList = dt1.AsEnumerable().Select(reader => new DSM_DocCategory
                        {
                            DocCategoryID = reader.GetString("DocCategoryID"),
                            OwnerID = reader.GetString("OwnerID"),
                            DocCategorySL = reader.GetString("DocCategorySL"),
                            UDDocCategoryCode = reader.GetString("UDDocCategoryCode"),
                            DocCategoryName = reader.GetString("DocCategoryName"),
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
            return docCategoriesList;
        }

        public string ManipulateDocCategory(DSM_DocCategory category, string action, out string errorNumber)
        {
            errorNumber = String.Empty;
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;
                using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocCategory"))
                {
                    // Set parameters 
                    db.AddInParameter(dbCommandWrapper, "@DocCategoryID", SqlDbType.NVarChar, category.DocCategoryID);
                    db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, category.OwnerID);
                    db.AddInParameter(dbCommandWrapper, "@DocCategorySL", SqlDbType.NVarChar, category.DocCategorySL);
                    db.AddInParameter(dbCommandWrapper, "@UDDocCategoryCode", SqlDbType.NVarChar, category.UDDocCategoryCode);
                    db.AddInParameter(dbCommandWrapper, "@DocCategoryName", SqlDbType.NVarChar, DataValidation.TrimmedOrDefault(category.DocCategoryName));
                    db.AddInParameter(dbCommandWrapper, "@UserLevel", SqlDbType.Int, category.UserLevel);
                    db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, category.SetBy);
                    db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, category.ModifiedBy);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, category.Status);
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

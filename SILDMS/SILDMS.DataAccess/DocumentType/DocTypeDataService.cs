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
using SILDMS.DataAccessInterface.DocumentType;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccess.DocumentType
{
    public class DocTypeDataService : IDocTypeDataService
    {
        #region Fields
        private readonly string spErrorParam = "@p_Error";
        private readonly string spStatusParam = "@p_Status";
        #endregion

        public List<DSM_DocType> GetDocType(string id, string userID, out string errorNumber)
        {
            errorNumber = string.Empty;
            var docTypeList = new List<DSM_DocType>();
            var factory = new DatabaseProviderFactory();
            var db = factory.CreateDefault() as SqlDatabase;
            using (var dbCommandWrapper = db.GetStoredProcCommand("GetDocType"))
            {
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, userID);
                db.AddInParameter(dbCommandWrapper, "@DocTypeID", SqlDbType.VarChar, id);
                db.AddOutParameter(dbCommandWrapper, spErrorParam, DbType.Int32, 10);

                var ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, "@p_Error").IsNullOrZero())
                {
                    // Get the error number, if error occurred.
                    errorNumber = db.GetParameterValue(dbCommandWrapper, "@p_Error").PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count <= 0) return docTypeList;
                    var dt1 = ds.Tables[0];
                    docTypeList = dt1.AsEnumerable().Select(reader => new DSM_DocType
                    {
                        DocTypeID = reader.GetString("DocTypeID"),
                        OwnerID = reader.GetString("OwnerID"),
                        DocCategoryID = reader.GetString("DocCategoryID"),
                        DocTypeSL = reader.GetString("DocTypeSL"),
                        UDDocTypeCode = reader.GetString("UDDocTypeCode"),
                        DocTypeName = reader.GetString("DocTypeName"),
                        DocPreservationPolicy = reader.GetString("DocPreservationPolicy"),
                        DocPhysicalLocation = reader.GetString("DocPhysicalLocation"),
                        SetOn = string.Format("{0:dd/mm/yyyy}", Convert.ToString(reader.GetDateTime("SetOn"), CultureInfo.InvariantCulture)),
                        SetBy = reader.GetString("SetBy"),
                        ModifiedOn = string.Format("{0:dd/mm/yyyy}", reader.GetDateTime("ModifiedOn").ToString(CultureInfo.InvariantCulture)),
                        ModifiedBy = reader.GetString("ModifiedBy"),
                        Status = reader.GetInt16("Status"),
                        DocClassification = reader.GetInt32("DocClassification"),
                        ClassificationLevel = reader.GetInt32("ClassificationLevel")
                    }).ToList();
                }
            }
            return docTypeList;
        }

        public string ManipulateDocType(DSM_DocType docType, string action, out string errorNumber)
        {
            errorNumber = string.Empty;
            try
            {
                var factory = new DatabaseProviderFactory();
                var db = factory.CreateDefault() as SqlDatabase;
                using (var dbCommandWrapper = db.GetStoredProcCommand("SetDocType"))
                {
                    // Set parameters 
                    db.AddInParameter(dbCommandWrapper, "@DocTypeID", SqlDbType.NVarChar, docType.DocTypeID);
                    db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, docType.OwnerID);
                    db.AddInParameter(dbCommandWrapper, "@DocCategoryID", SqlDbType.NVarChar, docType.DocCategoryID);
                    db.AddInParameter(dbCommandWrapper, "@DocTypeSL", SqlDbType.NVarChar, docType.DocTypeSL);
                    db.AddInParameter(dbCommandWrapper, "@UDDocTypeCode", SqlDbType.NVarChar, docType.UDDocTypeCode);
                    db.AddInParameter(dbCommandWrapper, "@DocTypeName", SqlDbType.NVarChar, docType.DocTypeName);
                    db.AddInParameter(dbCommandWrapper, "@DocPreservationPolicy", SqlDbType.NVarChar, docType.DocPreservationPolicy);
                    db.AddInParameter(dbCommandWrapper, "@DocPhysicalLocation", SqlDbType.NVarChar, docType.DocPhysicalLocation);
                    db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, docType.SetBy);
                    db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, docType.ModifiedBy);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, docType.Status);
                    db.AddInParameter(dbCommandWrapper, "@ClassificationLevel", SqlDbType.Int, docType.ClassificationLevel);
                    db.AddInParameter(dbCommandWrapper, "@DocClassification", SqlDbType.Int, docType.DocClassification);
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
